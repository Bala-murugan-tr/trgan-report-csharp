using System;

namespace TrganReport.Exceptions;
internal class ChartConfigurationException : TrganException {
    public ChartConfigurationException(string message) : base(message) { }
    public ChartConfigurationException(string message, Exception innerException) : base(message, innerException) { }
}
