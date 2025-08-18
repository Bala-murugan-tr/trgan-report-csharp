using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using TrganReport.Composer;
using TrganReport.Configs;
using TrganReport.Models;
using TrganReport.Table;
using TrganReport.Utils;

namespace TrganReport;
/// <summary>
/// Class for building and generating Trgan reports.
/// Primary entry point for constructing structured execution reports.
/// </summary>
public class TrganHtmlReport {

    private TrganTable trganTable;
    private readonly ConcurrentQueue<TrganContainer> _containers = [];
    private int _defaultContainerAdded = 0;
    private readonly Lazy<TrganContainer> _defaultContainer;
    internal bool useTrganTable = false;
    internal ReportWriter Writer { get; private set; }
    /// <summary>
    /// Represents aggregated statistics for a report, including counts of test containers, tests, and steps categorized by status.
    /// Useful for generating summary views, visual indicators, and analytics in the final report.
    /// </summary>
    public ReportStats ReportStats { get; } = new();
    /// <summary>Gets the formatted duration of the report execution. Populated during report generation.</summary>
    internal string ReportDuration { get; private set; } = null!;
    /// <summary>
    /// Centralized configuration for Trgan reports.
    /// Provides unified access to all configurations.
    /// </summary>
    public ReportConfig Config { get; } = new();
    /// <summary>Metadata section for the report, including author, environment, and custom fields.</summary>
    public MetaInfo MetaInfo { get; } = new();
    /// <summary>Gets the timestamp when the report was instantiated.</summary>
    public DateTime ReportStartTime { get; } = DateTime.Now;
    /// <summary> Gets the output file path for the Trgan report set during initialization.</summary>
    public string OutputPath { get; private set; }

    /// <summary>Initializes a new instance of the <see cref="TrganHtmlReport"/> class with the specified output path.</summary>
    /// <param name="outputPath">The full file path where the report will be generated.</param>
    public TrganHtmlReport(string outputPath) {
        string fullPath = PathValidator.ValidateAndNormalize(outputPath);
        PathValidator.CreateDirectory(fullPath);
        OutputPath = fullPath;

        _defaultContainer = new Lazy<TrganContainer>(
            () => new TrganContainer(this, "default"),
            LazyThreadSafetyMode.ExecutionAndPublication);

        Writer = new ReportWriter(OutputPath);
        Writer.LoadHeader();
        Writer.StartScreenshotBlob();
    }

    /// <summary>
    /// Initializes and enables the Trgan table layout with the specified column count.
    /// </summary>
    /// <param name="columnCount">Number of columns in the table.</param>
    /// <returns>A new <see cref="TrganTable"/> instance.</returns>
    public TrganTable TrganTable(int columnCount) {
        useTrganTable = true;
        trganTable = new TrganTable(columnCount);
        return trganTable;
    }
    internal TrganTable GetTrganTable() { return trganTable; }

    /// <summary>
    /// Creates a named container to group related tests.
    ///<list type = "bullet" >
    /// <item>In BDD-style testing, this corresponds to a <u>Feature</u>.</item>
    /// <item>In TDD-style testing, this typically maps to a <u>Test Class</u>.</item>
    /// </list>
    /// </summary>
    /// <param name="name">The name of the container.</param>
    /// <param name="categories">Categories or tag names</param>
    /// <returns>A new <see cref="TrganContainer"/> instance.</returns>
    public TrganContainer CreateContainer(string name, string[] categories = null) {
        TrganContainer container = new(this, name, categories);
        _containers.Enqueue(container);
        return container;
    }
    /// <summary>
    /// Creates a test directly to the report without any TrganContainers. 
    /// Useful when the report needs to be in scenario/test basis, instead of feature/testclass basis.
    /// <br> </br> <br> </br>
    /// For structured reports that follow a <b>Feature → Scenarios</b> hierarchy (e.g., BDD or class-based TDD),
    /// use <see cref="TrganContainer.CreateTest(TrganContainer, string, string[])"/> 
    /// </summary>
    /// <param name="testName">The name of the test.</param>
    /// <param name="categories">Optional categories or tag names for the test.</param>
    /// <returns>A new <see cref="TrganTest"/> instance.</returns>
    public TrganTest CreateTest(string testName, string[] categories = null) {
        TrganContainer defaultContainer = _defaultContainer.Value;
        if (Interlocked.CompareExchange(ref _defaultContainerAdded, 1, 0) == 0) {
            _containers.Enqueue(defaultContainer);
        }
        return defaultContainer.CreateTest(defaultContainer, testName, categories);
    }
    /// <summary>Generates the final Trgan Report file.</summary>
    public void Generate() {
        ReportDuration = TimeFormatter.Format(DateTime.Now - ReportStartTime);
        ReportBuilder.Build(this);
    }
    /// <summary>Gets a read-only list of all containers currently present in the report.</summary>
    public IReadOnlyList<TrganContainer> TrganContainers {
        get {
            return [.. _containers];
        }
    }

}
