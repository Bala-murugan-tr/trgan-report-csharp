using System;
using System.IO;
using TrganReport.Enums;
using TrganReport.Exceptions;
using TrganReport.Utils;

namespace TrganReport.Core;
/// <summary>
/// Represents a screenshot artifact that can be embedded in the report.
/// Supports multiple input formats including Base64 strings, byte arrays, and file paths.
/// Provides validation and conversion utilities for consistent rendering.
/// </summary>
public class Screenshot {
    /// <summary>
    /// Gets the optional description associated with the screenshot.
    /// Used for labeling or annotating the image in the report.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the screenshot content as a Base64-encoded string, if provided directly.
    /// </summary>
    public string Base64Content { get; init; }

    /// <summary>
    /// Gets the file path to the screenshot image, if loaded from disk.
    /// </summary>
    public string FilePath { get; init; }

    /// <summary>
    /// Gets the screenshot content as a byte array, if provided directly.
    /// </summary>
    public byte[] ByteArray { get; init; }

    /// <summary>
    /// Gets the format in which the screenshot was provided (Base64, byte array, or file path).
    /// </summary>
    internal ScreenshotType Type { get; set; }

    /// <summary>
    /// Creates a screenshot from a Base64 string.
    /// </summary>
    /// <param name="base64">The Base64-encoded image content.</param>
    /// <param name="description">Optional description for the screenshot.</param>
    /// <returns>A valid <see cref="Screenshot"/> instance.</returns>
    /// <exception cref="ScreenshotException">Thrown if the Base64 content is null or whitespace.</exception>
    public static Screenshot FromBase64(string base64, string description = null) {
        Screenshot screenshot = new(description) { Base64Content = base64, Type = ScreenshotType.Base64 };
        if (!screenshot.IsValid())
            throw new ScreenshotException("Invalid Base64 content for screenshot.");
        return screenshot;
    }

    /// <summary>
    /// Creates a screenshot from a byte array.
    /// </summary>
    /// <param name="bytes">The raw image bytes.</param>
    /// <param name="description">Optional description for the screenshot.</param>
    /// <returns>A valid <see cref="Screenshot"/> instance.</returns>
    /// <exception cref="ScreenshotException">Thrown if the byte array is null or empty.</exception>
    public static Screenshot FromBytes(byte[] bytes, string description = null) {
        Screenshot screenshot = new(description) { ByteArray = bytes, Type = ScreenshotType.ByteArray };
        if (!screenshot.IsValid()) {
            throw new ScreenshotException("Invalid byte array for screenshot.");
        }
        return screenshot;
    }

    /// <summary>
    /// Creates a screenshot from a file path.
    /// </summary>
    /// <param name="path">The path to the image file.</param>
    /// <param name="description">Optional description for the screenshot.</param>
    /// <returns>A valid <see cref="Screenshot"/> instance.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    public static Screenshot FromFile(string path, string description = null) {
        Screenshot screenshot = new(description) { FilePath = path, Type = ScreenshotType.FilePath };
        if (!screenshot.IsValid())
            throw new FileNotFoundException("Screenshot file not found.", path);
        return screenshot;
    }
    /// <summary>
    /// Converts the screenshot to a Base64-encoded string.
    /// Throws an exception if the screenshot is invalid or missing.
    /// </summary>
    /// <returns>The image content as a Base64 string.</returns>
    /// <exception cref="ScreenshotException">Thrown if the screenshot is invalid or missing.</exception>
    internal string GetAsDeflatedBase64() {
        if (!IsValid())
            throw new ScreenshotException("Screenshot data missing or invalid");

        return Type switch {
            ScreenshotType.ByteArray => ImageUtility.SCaptureDeflatedBase64(ByteArray),
            ScreenshotType.Base64 => ImageUtility.SCaptureDeflatedPNGBase64(Convert.FromBase64String(Base64Content)),
            _ => ImageUtility.SCaptureDeflatedBase64(File.ReadAllBytes(FilePath)),
        };

    }

    /// <summary>
    /// Determines whether the screenshot contains valid data based on its type.
    /// </summary>
    /// <returns><c>true</c> if the screenshot is valid; otherwise, <c>false</c>.</returns>
    internal bool IsValid() {
        return Type switch {
            ScreenshotType.Base64 => !string.IsNullOrWhiteSpace(Base64Content),
            ScreenshotType.ByteArray => ByteArray?.Length > 0,
            ScreenshotType.FilePath => File.Exists(FilePath),
            _ => false
        };
    }

    private Screenshot(string description) {
        Title = (description ?? string.Empty).Trim();
    }
}