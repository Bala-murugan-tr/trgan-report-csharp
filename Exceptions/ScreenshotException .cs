using System;

namespace TrganReport.Exceptions;
internal class ScreenshotException : TrganException {
    public ScreenshotException(string msg) : base(msg) { }
    public ScreenshotException(string message, Exception innerException) : base(message, innerException) { }
}
