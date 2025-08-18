using System;
using System.IO;
using TrganReport.Exceptions;

namespace TrganReport.Utils;
/// <summary>
/// Validates and normalizes file paths for reports.
/// Throws ReportPathException on any invalid input or I/O issue.
/// </summary>
internal static class PathValidator {
    /// <summary>
    /// Ensures the raw path is non-empty, well-formed, absolute, 
    /// has a directory component, and a valid file name.
    /// Returns the fully qualified path.
    /// </summary>
    internal static string ValidateAndNormalize(string rawPath) {
        if (string.IsNullOrWhiteSpace(rawPath))
            throw new ReportPathException("Report path cannot be null, empty, or whitespace.");

        if (rawPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ReportPathException($"Report path contains invalid path character: '{rawPath}'.");

        string fullPath;
        try {
            fullPath = Path.GetFullPath(rawPath);
        } catch (ArgumentException ex) {
            throw new ReportPathException($"Invalid characters in report path: '{rawPath}'.", ex);
        } catch (NotSupportedException ex) {
            throw new ReportPathException($"Unsupported path format: '{rawPath}'.", ex);
        } catch (PathTooLongException ex) {
            throw new ReportPathException($"Report path is too long: '{rawPath}'.", ex);
        }

        if (!Path.IsPathRooted(fullPath))
            throw new ReportPathException($"Report path must be absolute: '{fullPath}'.");

        string dir = Path.GetDirectoryName(fullPath);
        if (string.IsNullOrWhiteSpace(dir))
            throw new ReportPathException($"Report path must include a directory: '{fullPath}'.");

        string fileName = Path.GetFileName(fullPath);
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ReportPathException($"Report path must include a file name: '{fullPath}'.");

        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            throw new ReportPathException($"Report file name contains invalid character(s): '{fileName}'.");

        return fullPath;
    }

    /// <summary>
    /// Creates the directory portion of the given path, 
    /// wrapping any I/O or permission errors.
    /// </summary>
    internal static void CreateDirectory(string fullPath) {
        string directory = Path.GetDirectoryName(fullPath)!;
        try {
            Directory.CreateDirectory(directory);
        } catch (IOException ex) {
            throw new ReportPathException($"I/O error creating report directory '{directory}'.", ex);
        } catch (UnauthorizedAccessException ex) {
            throw new ReportPathException($"Access denied creating report directory '{directory}'.", ex);
        } catch (System.Security.SecurityException ex) {
            throw new ReportPathException($"Security error creating report directory '{directory}'.", ex);
        }
    }
}
