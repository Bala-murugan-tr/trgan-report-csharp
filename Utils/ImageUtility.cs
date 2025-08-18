using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using TrganReport.Configs;

namespace TrganReport.Utils;
/// <summary>
/// Provides utilities for resizing, encoding, compressing, and Base64-encoding images.
/// Supports PNG and JPEG formats with optional scaling and quality control.
/// </summary>
internal static class ImageUtility {

    /// <summary>
    /// Resizes a PNG image (from png byte array), re-encodes it as PNG, compresses using Deflate,
    /// and returns the result as a Base64 string.
    /// </summary>
    /// <param name="bytes">Original PNG image bytes.</param>
    /// <returns>Base64 string of deflated PNG bytes, or null on failure.</returns>
    internal static string SCaptureDeflatedPNGBase64(byte[] bytes) {
        try {
            using MemoryStream inputStream = new(bytes);
            using Bitmap originalImage = new(inputStream);
            using MemoryStream pngStream = new();
            using Bitmap resizedImage = ResizeImage(originalImage);
            resizedImage.Save(pngStream, ImageFormat.Png);
            return DeflateAndBase64(pngStream);
        } catch {
            return null;
        }
    }
    /// <summary>
    /// Resizes a JPEG image (from byte array), re-encodes it as JPEG with specified quality,
    /// compresses using Deflate, and returns the result as a Base64 string.
    /// </summary>
    /// <param name="bytes">Original image bytes (any format supported by Bitmap).</param>\
    /// <returns>Base64 string of deflated JPEG bytes, or null on failure.</returns>
    internal static string SCaptureDeflatedBase64(byte[] bytes) {
        try {
            using MemoryStream origStream = new(bytes);
            using Bitmap origBmp = new(origStream);
            using MemoryStream jpegStream = new();
            using Bitmap resizedBmp = ResizeImage(origBmp);
            resizedBmp.Save(jpegStream, jpgEncoder, CachedJpegParams);
            return DeflateAndBase64(jpegStream);
        } catch {
            return null;
        }
    }

    /// <summary>
    /// Compresses a MemoryStream using Deflate and returns the result as a Base64 string.
    /// </summary>
    /// <param name="inputStream">Stream containing image bytes.</param>
    /// <returns>Base64 string of deflated data.</returns>
    private static string DeflateAndBase64(MemoryStream inputStream) {
        inputStream.Position = 0;
        using MemoryStream deflatedStream = new(capacity: (int)inputStream.Length / 2 + 16);
        using (DeflateStream deflate = new(deflatedStream, CompressionLevel.Optimal, leaveOpen: true)) {
            inputStream.CopyTo(deflate);
        }
        // Avoid ToArray() copy by grabbing the internal buffer
        if (deflatedStream.TryGetBuffer(out ArraySegment<byte> buf))
            return Convert.ToBase64String(buf.Array, buf.Offset, buf.Count);
        return Convert.ToBase64String(deflatedStream.ToArray());
    }
    /// <summary>
    /// Resizes a Bitmap using high-quality bilinear interpolation.
    /// </summary>
    /// <param name="original">Original Bitmap.</param>
    /// <param name="scaleFactor">Scale factor to apply.</param>
    /// <returns>Resized Bitmap.</returns>
    /// <remarks>Time consuming task</remarks>
    private static Bitmap ResizeImage(Bitmap original, double scaleFactor = 1.0) {
        if (scaleFactor == 0.0 || scaleFactor == 1.0) return (Bitmap)original.Clone();
        int width = (int)(original.Width * scaleFactor);
        int height = (int)(original.Height * scaleFactor);
        Bitmap resized = new(width, height);
        using Graphics g = Graphics.FromImage(resized);
        g.InterpolationMode = InterpolationMode.HighQualityBilinear;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.DrawImage(original, 0, 0, width, height);
        return resized;
    }


    /// <summary>
    /// Finds the image encoder for a given image format (e.g., JPEG, PNG).
    /// </summary>
    /// <param name="format">Target image format.</param>
    /// <returns>Matching ImageCodecInfo, or null if not found.</returns>
    private static ImageCodecInfo GetEncoder(ImageFormat format) =>
        ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == format.Guid);

    public static long ImageQuality = ImageConfig.ImageQuality;

    private static readonly EncoderParameters CachedJpegParams = new(1) {
        Param = { [0] = new EncoderParameter(Encoder.Quality, ImageQuality) }
    };

    private static readonly ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
}
