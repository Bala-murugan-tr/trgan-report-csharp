using System;
using System.Collections.Generic;

namespace TrganReport.Models;

/// <summary>
/// Represents metadata associated with a test execution run.
/// This includes environment details, versioning, and custom fields for enhanced traceability.
/// </summary>
public sealed class MetaInfo {
    /// <summary>
    /// The name of the executor or automation agent.
    /// </summary>
    public string Executor { get; set; } = "Automation Tester";

    /// <summary>
    /// The execution mode.
    /// </summary>
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    /// The date of execution
    /// </summary>
    public string Date { get; set; } = DateTime.Now.ToString("d/M/yyyy");

    /// <summary>
    /// The sprint identifier or label associated with the test run.
    /// </summary>
    public string Sprint { get; set; } = string.Empty;

    /// <summary>
    /// The build number associated with the test run.
    /// </summary>
    public string Build { get; set; } = string.Empty;

    /// <summary>
    /// The execution environment (e.g., QA, Staging, Production).
    /// </summary>
    public string Environment { get; set; } = string.Empty;

    /// <summary>
    /// The release version associated with the test run.
    /// </summary>
    public string ReleaseVersion { get; set; } = string.Empty;

    /// <summary>
    /// The commit hash from the vcs.
    /// </summary>
    public string CommitHash { get; set; } = string.Empty;

    /// <summary>
    /// The branch name from which the test was executed.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// A descriptive summary of the changes, including details, messages, or reasons for the current execution.
    /// This information is always attached last in the report for maximum visibility.
    /// </summary>
    public string ChangeLogSummary { get; set; } = string.Empty;

    /// <summary>
    /// Custom metadata fields to be included in the report's meta information section.
    /// </summary>
    public Dictionary<string, string> CustomFields { get; set; } = [];
}
