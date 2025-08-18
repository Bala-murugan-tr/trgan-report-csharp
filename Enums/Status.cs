namespace TrganReport.Enums;
/// <summary>
/// Represents the execution status of a test, step, or container within TrganReport.
/// </summary>
public enum Status {
    /// <summary>
    /// No status has been assigned. Placeholder before execution.
    /// </summary>
    NONE,

    /// <summary>
    /// The container, test or step completed successfully without errors.
    /// </summary>
    PASS,

    /// <summary>
    /// The  container, test or step was intentionally skipped due to conditional execution or disabled scenarios.
    /// </summary>
    SKIP,

    /// <summary>
    /// The  container, test or step failed due to an error or unmet expectation.
    /// </summary>
    FAIL
}

