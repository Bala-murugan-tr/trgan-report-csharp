using System;

namespace TrganReport.Exceptions;
internal class ReportPathException : TrganException {
    public ReportPathException(string msg) : base(msg) { }
    public ReportPathException(string message, Exception innerException) : base(message, innerException) { }

}
