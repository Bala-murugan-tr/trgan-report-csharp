using System;

namespace TrganReport.Exceptions;
/// <summary>
/// Represents errors that occur within the TrganReport framework.
/// This is the base exception type for reporting-related failures.
/// </summary>
internal class TrganException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="TrganException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    internal TrganException(string message) : base(message) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="TrganException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    internal TrganException(string message, Exception inner) : base(message, inner) { }

}
