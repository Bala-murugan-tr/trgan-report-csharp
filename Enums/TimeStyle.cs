namespace TrganReport.Enums;
/// <summary>
/// Specifies the format style for rendering DateTime values.
/// </summary>
public enum TimeStyle {
    /// <summary>
    /// Shows only the time in 24-hour format with hours, minutes, and seconds.
    /// Pattern: "HH:mm:ss"
    /// </summary>
    TimeOnly,
    /// <summary>
    /// Shows the full date followed by the time in 24-hour format.
    /// Pattern: "yyyy-MM-dd HH:mm:ss"
    /// </summary>
    DateAndTime,
    /// <summary>
    /// Shows only the time in 12-hour format with AM/PM designator.
    /// Pattern: "hh:mm:ss tt"
    /// </summary>
    TimeAmPm
}
