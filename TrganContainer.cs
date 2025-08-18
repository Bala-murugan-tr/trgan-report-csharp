using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TrganReport.Models;

namespace TrganReport;
/// <summary>
/// Represents a logical container for grouping related test executions.
///<list type = "bullet" >
/// <item>In BDD-style testing, this corresponds to a <u>Feature</u>.</item>
/// <item>In TDD-style testing, this typically maps to a <u>Test Class</u>.</item>
/// </list>
/// Each container holds a thread-safe list of associated tests.
/// </summary>
/// <param name="report">Function Caller</param>
/// <param name="containerName">
/// The name of the container. In BDD, this is the feature name; in TDD, it's the test class name.
/// </param>
/// <param name="categories">
/// Optional categories or tags used to classify the container.
/// These may represent modules, priorities, or custom labels.
/// </param>
public class TrganContainer(TrganHtmlReport report, string containerName, string[] categories = null) {
    internal TrganHtmlReport report = report;
    /// <summary>
    /// Gets or sets the execution metadata for the container.
    /// Includes name, categories, start time, and status.
    /// </summary>
    internal ExecutionDetails ContainerDetails { get; private set; } = new ExecutionDetails() {
        Name = containerName,
        Category = categories ?? [],
        StartTime = DateTime.Now,
        Status = 0
    };

    private readonly ConcurrentQueue<TrganTest> _tests = [];
    /// <summary>
    /// Gets a read-only list of tests associated with this container.
    /// </summary>
    public IReadOnlyList<TrganTest> Tests {
        get { return [.. _tests]; }
    }
    /// <summary>
    /// Creates and adds a new Test to the container.
    /// Thread-safe operation that links the test to the container.
    /// </summary>
    /// <param name="name">The name of the test.</param>
    /// <returns>A new <see cref="TrganTest"/> instance.</returns>
    public TrganTest CreateTest(string name) {
        return CreateTest(this, name, null);
    }
    internal TrganTest CreateTest(TrganContainer container, string name, string[] categories = null) {
        TrganTest test = new(container, name, categories);
        _tests.Enqueue(test);
        return test;
    }

}
