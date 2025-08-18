namespace TrganReport.Enums;
/// <summary>
/// Represents the type of charts used in TrganReport.
/// </summary>
public enum Chart {
    /// <summary>
    /// A top-level container grouping multiple tests.
    /// </summary>
    Container = 1,

    /// <summary>
    /// An individual test case or unit.
    /// Used to represent a single logical test within the report.
    /// </summary>
    Test = 2,

    /// <summary>
    /// A granular execution step within a test.
    /// </summary>
    Step = 3
}
