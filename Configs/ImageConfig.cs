using TrganReport.Exceptions;

namespace TrganReport.Configs;
/// <summary>
/// Provides configuration settings for image processing.
/// </summary>
internal sealed class ImageConfig {
    private static long _imageQuality = 75L;
    /// <summary>
    /// Controls the image quality used during screenshot generation.
    /// Throws <see cref="ScreenshotException"/> if the quality value is outside the valid range (0–100).
    /// </summary>
    internal static long ImageQuality {
        get => _imageQuality;
        set {
            if (value <= 100L && value >= 0L)
                _imageQuality = value;
            else
                throw new ScreenshotException($"Invalid Image Quality: {value}, can set only between 100L to 0L");
        }
    }
}