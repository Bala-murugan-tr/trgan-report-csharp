using System;
using System.Collections.Generic;
using TrganReport.Core;
using TrganReport.Enums;

namespace TrganReport;
/// <summary>
/// Represents a single step within a Trgan test. Captures 
/// logs, status, and screenshots.
/// </summary>
public class TrganStep {

    /// <summary>
    /// Initializes a step with a reference to the parent test.
    /// </summary>
    /// <param name="test">The parent <see cref="TrganTest"/> instance.</param>
    public TrganStep(TrganTest test) {
        Test = test;
    }
    /// <summary>
    /// Initializes a step with a Gherkin keyword and a parent test.
    /// </summary>
    /// <param name="keyword">The Gherkin keyword (e.g., Given, When, Then).</param>
    /// <param name="test">The parent <see cref="TrganTest"/> instance.</param>
    public TrganStep(GherkinKeyword keyword, TrganTest test) {
        Keyword = keyword;
        Test = test;
    }
    internal readonly TrganTest Test;
    /// <summary>The Gherkin keyword associated with this step.</summary>
    internal GherkinKeyword Keyword { get; private set; }
    internal Status Status { get; set; }
    internal string Description { get; set; } = null!;
    internal DateTime EndTime { get; set; } = DateTime.Now;
    internal DateTime StartTime { get; set; } = DateTime.Now;
    /// <summary> A collection of log entries associated with this step. </summary>
    internal List<LogEntry> Logs = [];
    internal int ScreenshotId { get; set; }
    internal string ScreenshotTitle { get; set; }
    private void AddScreenshot(Screenshot screenshot) {
        if (screenshot == null) return;
        int ind = Test.container.report.Writer.WriteScreenshot(screenshot.GetAsDeflatedBase64());
        ScreenshotId = ind;
        ScreenshotTitle = screenshot.Title;
    }
    /// <summary>
    /// Logs the status and optional screenshot for the step.
    /// Also updates the parent test's status and end time.
    /// </summary>
    /// <param name="status">The status to log.</param>
    /// <param name="message">An optional write log message</param>
    /// <param name="screenshot">An optional screenshot.</param>
    public void Log(Status status, string message = null, Screenshot screenshot = null) {
        Status = status;
        AddScreenshot(screenshot);
        EndTime = DateTime.Now;
        Test.Status = status;
        Test.testDetails.EndTime = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(message) ){
            Logs.Add(new LogEntry(this, message, null, false));
        }
    }
    /// <summary>
    /// Writes a log entry with optional screenshot and timestamp.
    /// Also updates the parent test's end time.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="screenshot">An optional screenshot.</param>
    /// <param name="timeStamp">Whether to include a timestamp in the log.</param>
    public void Write(string message = "", Screenshot screenshot = null, bool timeStamp = true) {
        Logs.Add(new LogEntry(this, message, screenshot, timeStamp));
        Test.testDetails.EndTime = DateTime.Now;
    }
    /// <summary>
    /// Logs a passing status with an optional screenshot.
    /// </summary>
    /// <param name="screenshot">An optional screenshot to attach.</param>
    /// <param name="message">An optional write log message</param>
    public void Pass(string message=null, Screenshot screenshot = null) => Log(Status.PASS, message, screenshot);

    /// <summary>
    /// Logs a failing status with an optional screenshot.
    /// </summary>
    /// <param name="screenshot">An optional screenshot to attach.</param>
    /// <param name="message">An optional write log message</param>
    public void Fail(string message = null, Screenshot screenshot = null) => Log(Status.FAIL, message, screenshot);

    /// <summary>
    /// Logs a skipped status with an optional screenshot.
    /// </summary>
    /// <param name="screenshot">An optional screenshot to attach.</param>
    /// <param name="message">An optional write log message</param>
    public void Skip(string message = null, Screenshot screenshot = null) => Log(Status.SKIP, message, screenshot);

}