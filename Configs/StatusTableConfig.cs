using TrganReport.Enums;

namespace TrganReport.Configs;
/// <summary>
/// Configuration settings for controlling the visibility of specific columns
/// in a status table, such as category, end time, and duration.
/// </summary>
internal sealed class StatusTableConfig {
    private bool _showCategory = true;
    private bool _showEndTime = true;
    private bool _showDuration = true;

    /// <summary>
    /// Gets or sets a value indicating whether the "Category" column should be displayed in Execution Status table.
    /// Default is <c>true</c>.
    /// </summary>
    public bool ShowCategory {
        get => _showCategory;
        set => _showCategory = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the "End Time" column should be displayed in Execution Status table.
    /// Default is <c>true</c>.
    /// </summary>
    public bool ShowEndTime {
        get => _showEndTime;
        set => _showEndTime = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the "Duration" column should be displayed in Execution Status table.
    /// Default is <c>true</c>.
    /// </summary>
    public bool ShowDuration {
        get => _showDuration;
        set => _showDuration = value;
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
    internal TimeStyle TimeStyle { get; set; } = TimeStyle.TimeAmPm;
}

