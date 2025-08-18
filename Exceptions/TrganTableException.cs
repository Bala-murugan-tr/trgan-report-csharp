using System;

namespace TrganReport.Exceptions;
internal class TrganTableException : TrganException {
    public TrganTableException(string msg) : base(msg) { }
    public TrganTableException(string message, Exception innerException) : base(message, innerException) { }

}
