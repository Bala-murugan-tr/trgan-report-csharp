using System;
using System.Collections.Generic;
using TrganReport.Core;
using TrganReport.Enums;
using TrganReport.Models;

namespace TrganReport;
/// <summary>
/// Represents a single test case within a report.
/// Encapsulates metadata, execution timing, status, and associated steps.
/// </summary>
/// <param name="container">Parent execution container </param>
/// <param name="trgantestName">The name of the test case.</param>
/// <param name="categories">Optional categories or tags associated with the test.</param>
public class TrganTest(TrganContainer container, string trgantestName, string[] categories = null!) {
    internal TrganContainer container = container;
    internal ExecutionDetails testDetails = new() {
        Name = trgantestName,
        Category = categories ?? [],
        StartTime = DateTime.Now,
        EndTime = DateTime.Now,
        Status = 0
    };

    private readonly List<TrganStep> _steps = [];
    private TrganStep _step;

    internal IReadOnlyList<TrganStep> Steps {
        get { return [.. _steps]; }
    }
    /// <summary>
    /// Gets or sets the current status of the test.
    /// Automatically propagates to the parent container.
    /// </summary>
    internal Status Status {
        get => testDetails.Status;
        set {
            testDetails.Status = value;
            container.ContainerDetails.Status = value;
        }
    }
    /// <summary>
    /// Creates a new step.
    /// </summary>
    /// <param name="description">The step description.</param>
    /// <returns>The created <see cref="TrganStep"/>.</returns>
    public TrganStep CreateStep(string description) {
        return CreateStep(GherkinKeyword.None, description);
    }
    /// <summary>
    /// Creates a new step with a specific Gherkin keyword.
    /// </summary>
    /// <param name="keyword">The Gherkin keyword (e.g., Given, When, Then).</param>
    /// <param name="description">The step description.</param>
    /// <returns>The created <see cref="TrganStep"/>.</returns>
    public TrganStep CreateStep(GherkinKeyword keyword, string description) {
        TrganStep step = new(keyword, this) {
            Description = description
        };

        _steps.Add(step);
        UpdateEndTimes();
        _step = step;
        return step;
    }
    internal int ScreenshotId { get; set; }
    private void AddScreenshot(Screenshot screenshot) {
        if (screenshot != null) {
            int ind = container.report.Writer.WriteScreenshot(screenshot.GetAsDeflatedBase64());
            ScreenshotId = ind;
        }
    }
    /// <summary>
    /// Adds a new step with status, message, and optional screenshot.
    /// </summary>
    /// <param name="status">The step status.</param>
    /// <param name="message">The step message.</param>
    /// <param name="screenshot">Optional screenshot for the step.</param>
    /// <returns>The created <see cref="TrganStep"/>.</returns>
    public void AddStep(Status status, string message, Screenshot screenshot = null) {
        AddStep(status, GherkinKeyword.None, message, screenshot);

    }
    /// <summary>
    /// Adds a new step with status, gherkin keyword, message, and optional screenshot.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="gherkin"></param>
    /// <param name="message"></param>
    /// <param name="screenshot"></param>
    /// <returns></returns>
    public void AddStep(Status status, GherkinKeyword gherkin, string message, Screenshot screenshot = null) {
        DateTime earlyTime = testDetails.EndTime;
        TrganStep step = CreateStep(gherkin, message);
        step.Status = status;
        step.StartTime = earlyTime;
        AddScreenshot(screenshot);
        Status = status;
        step.EndTime = DateTime.Now;
        testDetails.EndTime = DateTime.Now;
    }
    /// <summary>
    /// Mark the test status with given <see cref="Status"/>
    /// </summary>
    /// <param name="status"></param>
    public void Log(Status status) {
        Status = status;
        UpdateEndTimes();
    }
    /// <summary>
    /// Marks the test as PASS
    /// </summary>
    public void Pass() => Log(Status.PASS);
    /// <summary>
    /// Marks the test as FAIL
    /// </summary>
    public void Fail() => Log(Status.FAIL);
    /// <summary>
    /// Marks the test as SKIP
    /// </summary>
    public void Skip() => Log(Status.SKIP);
    /// <summary>
    /// Logs a new step with status, message, and optional screenshot.
    /// </summary>
    /// <param name="status">The step status.</param>
    /// <param name="message">The step message.</param>
    /// <param name="screenshot">Optional screenshot.</param>
    public void Log(Status status, string message, Screenshot screenshot = null) => AddStep(status, message, screenshot);


    /// <summary>
    /// Logs a new passing step with the specified message and optional screenshot.
    /// </summary>
    /// <param name="message">The message describing the successful step.</param>
    /// <param name="screenshot">An optional screenshot to attach to the step.</param>
    public void Pass(string message, Screenshot screenshot = null) => AddStep(Status.PASS, message, screenshot);

    /// <summary>
    /// Logs a new failing step with the specified message and optional screenshot.
    /// </summary>
    /// <param name="message">The message describing the failure.</param>
    /// <param name="screenshot">An optional screenshot to attach to the step.</param>
    public void Fail(string message, Screenshot screenshot = null) => AddStep(Status.FAIL, message, screenshot);

    /// <summary>
    /// Logs a new skipped step with the specified message and optional screenshot.
    /// </summary>
    /// <param name="message">The message explaining why the step was skipped.</param>
    /// <param name="screenshot">An optional screenshot to attach to the step.</param>
    public void Skip(string message, Screenshot screenshot = null) => AddStep(Status.SKIP, message, screenshot);

    /// <summary>
    /// Updates the end time for this test and its container.
    /// Called automatically when steps are added.
    /// </summary>
    private void UpdateEndTimes() {
        DateTime now = DateTime.Now;
        testDetails.EndTime = now;
        container.ContainerDetails.EndTime = now;
    }

}
