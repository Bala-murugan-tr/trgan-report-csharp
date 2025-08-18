using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrganReport.Exceptions;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Streams HTML report content, including screenshot blobs(asynchronously) and structured sections.
/// </summary>
internal sealed class ReportWriter : IAsyncDisposable, IDisposable {
    private int _id;
    private const int BatchSize = 200;
    private bool _shutdownInitiated;
    private readonly StreamWriter _writer;
    private readonly Task _consumerTask;
    private readonly Channel<List<string>> _channel;
    private readonly List<string> _batch = [];
    private readonly CancellationTokenSource _cts = new();
    private readonly object _lock = new();
    private readonly object _shutdownLock = new();

    internal ReportWriter(string outputPath) {
        _writer = CreateWriter(outputPath);
        _channel = Channel.CreateUnbounded<List<string>>(new UnboundedChannelOptions {
            SingleReader = true,
            SingleWriter = false
        });
        _consumerTask = Task.Run(() => ConsumeBatchesAsync(_cts.Token));
    }

    internal void LoadHeader(bool offlineMode = false) {
        _writer.WriteLine("<!DOCTYPE html>\r\n<html lang=\"en\">");
        if (!offlineMode) {
            string header = ReportTemplates.Header
                .Replace("/*ICON*/", AssetPath.FaviconsUrl)
                .Replace("/*SITE*/", AssetPath.CdnBaseUrl)
                .Replace("/*SRC*/", AssetPath.StyleCssUrl);
            _writer.Write(header);
        } else
            _writer.Write(ReportTemplates.Header);
    }
    internal void StartScreenshotBlob() {
        _writer.WriteLine("<body>");
        _writer.WriteLine(" <script id=\"screenshot-data\" type=\"application/json\">");
        _writer.WriteLine(" {");
    }
    internal void LoadFooter(bool offlineMode) {
        _writer.WriteLine("");
        if (!offlineMode)
            _writer.WriteLine($@"  <script src=""{AssetPath.ScriptJsUrl}""></script>");
        _writer.WriteLine("</body>");
        _writer.WriteLine("</html>");
    }
    internal int WriteScreenshot(string content) {
        int currentId = Interlocked.Increment(ref _id);
        string line = $"      \"{currentId}\": \"{content}\",";

        List<string> toFlush = null;
        lock (_lock) {
            _batch.Add(line);
            if (_batch.Count >= BatchSize) {
                toFlush = [.. _batch];
                _batch.Clear();
            }
        }

        if (toFlush is not null) {
            _channel.Writer.TryWrite(toFlush);
        }

        return currentId;
    }

    internal void CloseScreenshotBlob() {
        List<string> toFlush = null;
        lock (_lock) {
            if (_batch.Count > 0) {
                toFlush = [.. _batch];
                _batch.Clear();
            }
        }

        if (toFlush is not null) {
            _channel.Writer.TryWrite(toFlush);
        }

        _channel.Writer.Complete();
        _consumerTask.GetAwaiter().GetResult();

        _writer.WriteLine("      \"end\": \"end\"");
        _writer.WriteLine("    }");
        _writer.WriteLine("  </script>");
    }

    /// <summary>
    /// Write html content, 
    /// close out the screenshot-data blob and shut everything down.
    /// </summary>
    /// <param name="content">HTML content to append after screenshots.</param>
    internal void WriteContent(string content) {
        CloseScreenshotBlob();
        _writer.Write(content);
    }
    internal void Close() {
        Dispose();
    }

    private async Task ConsumeBatchesAsync(CancellationToken ct) {
        try {
            await foreach (List<string> batch in _channel.Reader.ReadAllAsync(ct)) {
                foreach (string line in batch)
                    await _writer.WriteLineAsync(line).ConfigureAwait(false);
                await _writer.FlushAsync(ct).ConfigureAwait(false);
            }
        } catch (OperationCanceledException) {
        } catch (Exception ex) {
            Console.Error.WriteLine($"[ReportWriter] Error in consumer: {ex}");
        }
    }
    private static StreamWriter CreateWriter(string fullPath) {
        try {
            return new StreamWriter(fullPath, append: false, encoding: Encoding.UTF8, bufferSize: 8192);
        } catch (IOException ex) {
            throw new ReportPathException($"I/O error initializing report file '{fullPath}'.", ex);
        } catch (UnauthorizedAccessException ex) {
            throw new ReportPathException($"Access denied initializing report file '{fullPath}'.", ex);
        } catch (System.Security.SecurityException ex) {
            throw new ReportPathException($"Security error initializing report file '{fullPath}'.", ex);
        }
    }
    /// <summary>
    /// Asynchronously shuts down the writer and consumer task.
    /// </summary>
    public async Task ShutdownAsync() {
        _cts.Cancel();

        try {
            await _consumerTask.ConfigureAwait(false);
        } catch { }

        _writer.Dispose();
        _cts.Dispose();
    }
    /// <summary>
    /// Synchronously disposes resources and cancels background tasks.
    /// </summary>
    public void Dispose() {
        Shutdown();
        GC.SuppressFinalize(this);
    }
    private void Shutdown() {
        lock (_shutdownLock) {
            if (_shutdownInitiated) return;
            _shutdownInitiated = true;

            // Cancel consumer (if not already completed)
            _cts.Cancel();

            try {
                _consumerTask.GetAwaiter().GetResult();
            } catch { }

            // Release resources
            _writer.Dispose();
            _cts.Dispose();
        }
    }
    /// <summary>
    /// Asynchronously disposes resources used by the current instance.
    /// Ensures graceful shutdown of internal components such as writers and cancellation tokens.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous disposal operation.</returns>
    public async ValueTask DisposeAsync() {
        Shutdown();
        GC.SuppressFinalize(this);
        await Task.CompletedTask;
    }
}
