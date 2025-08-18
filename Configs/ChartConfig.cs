using System;
using System.Collections.Generic;
using System.Linq;
using TrganReport.Enums;
using TrganReport.Exceptions;

namespace TrganReport.Configs;
/// <summary>
/// Represents configuration settings for chart titles and chart ordering
/// used in Trgan reports.
/// </summary>
internal sealed class ChartConfig {
    private string _containerChartTitle = "Execution Overview";
    private string _testChartTitle = "Test Overview";
    private string _stepsChartTitle = "Steps Overview";
    private List<Chart> _charts = [Chart.Container, Chart.Test, Chart.Step];
    /// <summary>
    /// Gets or sets the title for the container-level chart.
    /// </summary>
    /// <exception cref="ChartConfigurationException">Thrown when the title is null, empty, or whitespace.</exception>
    public string ContainerChartTitle {
        get => _containerChartTitle;
        set {
            if (string.IsNullOrWhiteSpace(value))
                throw new ChartConfigurationException("Container chart title cannot be null or empty.");
            _containerChartTitle = value;
        }
    }
    /// <summary>
    /// Gets or sets the title for the test-level chart.
    /// </summary>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when the title is null, empty, or whitespace.
    /// </exception>
    public string TestChartTitle {
        get => _testChartTitle;
        set {
            if (string.IsNullOrWhiteSpace(value))
                throw new ChartConfigurationException("Test chart title cannot be null or empty.");
            _testChartTitle = value;
        }
    }
    /// <summary>
    /// Gets or sets the title for the step-level chart.
    /// </summary>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when the title is null, empty, or whitespace.
    /// </exception>
    public string StepsChartTitle {
        get => _stepsChartTitle;
        set {
            if (string.IsNullOrWhiteSpace(value))
                throw new ChartConfigurationException("Steps chart title cannot be null or empty.");
            _stepsChartTitle = value;
        }
    }
    /// <summary>
    /// Sets the order in which charts should appear in the report.
    /// </summary>
    /// <param name="chart1">The first chart in the order.</param>
    /// <param name="chart2">The second chart in the order.</param>
    /// <param name="chart3">The third chart in the order.</param>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when duplicate charts are provided.
    /// </exception>
    public void SetChartOrder(Chart chart1, Chart chart2, Chart chart3) {
        List<Chart> chartList = [chart1, chart2, chart3];
        ValidateChartOrder(chartList);
        _charts = chartList;
    }
    /// <summary>
    /// Gets or sets the chart order used in the report.
    /// </summary>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when duplicate charts are provided.
    /// </exception>
    public List<Chart> ChartOrder {
        get => _charts;
        set {
            List<Chart> chartList = value;
            ValidateChartOrder(value);
            _charts = chartList;
        }
    }
    /// <summary>
    /// Validates that the chart list contains no duplicates.
    /// </summary>
    /// <param name="chartList">The list of charts to validate.</param>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when duplicate charts are detected.
    /// </exception>
    private static void ValidateChartOrder(List<Chart> chartList) {
        if (chartList.Distinct().Count() != chartList.Count) {
            List<Chart> duplicates = chartList
                .GroupBy(c => c)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            throw new ChartConfigurationException($"Duplicate charts in Chart Order: {string.Join(", ", duplicates)}");
        }
    }
}
