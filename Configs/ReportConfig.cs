using System.Collections.Generic;
using TrganReport.Enums;
using TrganReport.Exceptions;

namespace TrganReport.Configs;
/// <summary>
/// Centralized configuration for Trgan reports.
/// Provides unified access to all configurations.
/// </summary>
public class ReportConfig {
    private readonly ChartConfig chartConfig = new();
    private readonly StatusTableConfig statusTableConfig = new();
    private readonly ImageConfig imageConfig = new();
    /// <summary>
    /// Indicates whether the report should operate in offline mode.
    /// When true, all assets (screenshots, styles, scripts) are embedded directly into the HTML.
    /// When false, assets may be loaded externally via CDN.
    /// </summary>
    internal bool OfflineMode { get; set; } = false;
    /// <summary>
    /// The heading displayed at the top of the report. Default header is "Automation Report".
    /// </summary>
    /// <remarks>Prefer Uppercase</remarks>
    public string ReportHeading { get; set; } = "Automation Report";
    /// <summary>
    /// Enables emoji rendering in the meta summary's expanded section of the report. Indicators and visual cues include emojis for enhanced readability.
    /// </summary>
    public bool UseEmojis { get; set; } = true;
    /// <summary>
    /// Gets or sets a value indicating whether the "Category" column should be displayed in Execution Status table.
    /// Default is <c>true</c>.
    /// </summary>
    public bool ShowCategory {
        get => statusTableConfig.ShowCategory;
        set => statusTableConfig.ShowCategory = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the "End Time" column should be displayed in Execution Status table.
    /// Default is <c>true</c>.
    /// </summary>
    public bool ShowEndTime {
        get => statusTableConfig.ShowEndTime;
        set => statusTableConfig.ShowEndTime = value;
    }
    /// <summary>
    /// Gets or sets a value indicating whether the "Duration" column should be displayed in Execution Status table.
    /// Default is <c>true</c>.
    /// </summary>
    public bool ShowDuration {
        get => statusTableConfig.ShowDuration;
        set => statusTableConfig.ShowDuration = value;
    }
    /// <summary>
    /// Controls how timestamps are formatted throughout the report.
    /// Options include:
    /// <list type="bullet">
    /// <item>TimeOnly: HH:mm:ss (24-hour)</item>
    /// <item>DateAndTime: yyyy-MM-dd HH:mm:ss</item>
    /// <item>TimeAmPm: hh:mm tt (12-hour with AM/PM)</item>
    /// </list>
    /// </summary>
    public TimeStyle TimeStyle {
        get => statusTableConfig.TimeStyle;
        set => statusTableConfig.TimeStyle = value;
    }
    /// <summary>
    /// Gets or sets the title for the container-level chart.
    /// </summary>
    /// <exception cref="ChartConfigurationException">Thrown when the title is null, empty, or whitespace.</exception>
    public string TestChartTitle {
        get => chartConfig.ContainerChartTitle;
        set => chartConfig.ContainerChartTitle = value;
    }
    /// <summary>
    /// Gets or sets the title for the test-level chart.
    /// </summary>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when the title is null, empty, or whitespace.
    /// </exception>
    public string SubTestChartTitle {
        get => chartConfig.TestChartTitle;
        set => chartConfig.TestChartTitle = value;
    }

    /// <summary>
    /// Gets or sets the title for the step-level chart.
    /// </summary>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when the title is null, empty, or whitespace.
    /// </exception>
    public string StepChartTitle {
        get => chartConfig.StepsChartTitle;
        set => chartConfig.StepsChartTitle = value;
    }

    /// <summary>
    /// Gets or sets the chart order used in the report.
    /// </summary>
    /// <exception cref="ChartConfigurationException">
    /// Thrown when duplicate charts are provided.
    /// </exception>
    private List<Chart> ChartOrder {
        get => chartConfig.ChartOrder;
        set => chartConfig.ChartOrder = value;
    }
    /// <summary>
    /// Controls the image quality used during screenshot generation.
    /// Throws <see cref="ScreenshotException"/> if the quality value is outside the valid range (0–100).
    /// </summary>
    public long ImageQuality {
        get => ImageConfig.ImageQuality;
        set => ImageConfig.ImageQuality = value;
    }
}
