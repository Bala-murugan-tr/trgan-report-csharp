using TrganReport.Enums;

namespace TrganReport.Models;

/// <summary>
/// Represents aggregated statistics for a report, including counts of test containers, tests, and steps categorized by status.
/// Useful for generating summary views, visual indicators, and analytics in the final report.
/// </summary>
public sealed class ReportStats {
    // Test Container Stats
    /// <summary>Total number of test containers executed.</summary>
    public int TotalTestContainers { get; internal set; }
    /// <summary>Number of test containers that passed.</summary>
    public int PassedTestContainers { get; internal set; }
    /// <summary>Number of test containers that failed.</summary>
    public int FailedTestContainers { get; internal set; }
    /// <summary>Number of test containers that were skipped.</summary>
    public int SkippedTestContainers { get; internal set; }
    // Test Stats
    /// <summary>Total number of tests executed.</summary>
    public int TotalTests { get; internal set; }
    /// <summary>Number of tests that passed.</summary>
    public int PassedTests { get; internal set; }
    /// <summary>Number of tests that failed.</summary>
    public int FailedTests { get; internal set; }
    /// <summary>Number of tests that were skipped.</summary>
    public int SkippedTests { get; internal set; }
    // Step Stats
    /// <summary>Total number of steps executed.</summary>
    public int TotalSteps { get; internal set; }
    /// <summary>Number of steps that passed.</summary>
    public int PassedSteps { get; internal set; }
    /// <summary>Number of steps that failed.</summary>
    public int FailedSteps { get; internal set; }
    /// <summary>Number of steps that were skipped.</summary>
    public int SkippedSteps { get; internal set; }

    /// <summary>
    /// Updates the appropriate status counters based on the given type and execution status.
    /// This method is typically invoked during report generation to track pass/fail/skip metrics across different granularities.
    /// </summary>
    /// <param name="type">The category of the item being evaluated (e.g., step, test, container).</param>
    /// <param name="status">The execution result for the item (pass, fail, or skip).</param>
    internal void UpdateStat(StatusType type, Status status) {
        switch (type) {
            case StatusType.Step:
                if (status == Status.PASS) PassedSteps++;
                else if (status == Status.FAIL) FailedSteps++;
                else if (status == Status.SKIP) SkippedSteps++;
                break;

            case StatusType.Test:
                if (status == Status.PASS) PassedTests++;
                else if (status == Status.FAIL) FailedTests++;
                else if (status == Status.SKIP) SkippedTests++;
                break;

            case StatusType.Container:
                if (status == Status.PASS) PassedTestContainers++;
                else if (status == Status.FAIL) FailedTestContainers++;
                else if (status == Status.SKIP) SkippedTestContainers++;
                break;
        }
    }

}

