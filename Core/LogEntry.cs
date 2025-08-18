using System;

namespace TrganReport.Core;
/// <summary>
/// Represents a single log entry for a test step in the report.
/// Includes a timestamp, message, and screenshot for enhanced traceability.
/// </summary>
public class LogEntry {
    /// <summary>
    /// Gets the timestamp associated with the log entry.
    /// Defaults to the time of LogEntry object creation.
    /// </summary>
    internal string Timestamp { get; private set; }

    /// <summary>
    /// Gets or sets the descriptive message for the log entry.
    /// Typically used to capture step-level status or context.
    /// </summary>
    internal string Message { get; set; }

    internal int ScreenshotId { get; set; }
    internal string ScreenshotTitle { get; set; }
    private void AddScreenshot(Screenshot screenshot) {
        if (screenshot == null) return;
        int ind = _step.Test.container.report.Writer.WriteScreenshot(screenshot.GetAsDeflatedBase64());
        ScreenshotId = ind;
        ScreenshotTitle = screenshot.Title;
    }
    private readonly TrganStep _step;
    /// <summary>
    /// Initializes a new instance of the <see cref="LogEntry"/> class.
    /// </summary>
    /// <param name="step">function caller</param>
    /// <param name="message">Optional message describing the log context.</param>
    /// <param name="image">Optional screenshot to include in the log.</param>
    /// <param name="timestamp">
    /// If true, logs the timestamp of the log entry.
    /// </param>
    public LogEntry(TrganStep step, string message = "", Screenshot image = null, bool timestamp = true) {
        _step = step;
        Timestamp = timestamp ? DateTime.Now.ToString(@"hh:mm:ss tt") : "";
        Message = !string.IsNullOrEmpty(message) ? message : "";
        AddScreenshot(image);
    }
}


