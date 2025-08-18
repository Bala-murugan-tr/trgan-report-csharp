using System;
using TrganReport.Enums;

namespace TrganReport.Utils;
/// <summary>
/// Provides a human-readable formatting utility for <see cref="TimeSpan"/> values.
/// Converts durations into compact strings like "999ms" or "12h 59m 59s".
/// </summary>
public static class TimeFormatter {
    /// <summary>
    /// Formats a <see cref="TimeSpan"/> into a readable string.
    /// <list type="bullet">
    /// <item>Durations under 1 second are shown in milliseconds (e.g., "999 ms")</item>
    /// <item> Durations of 1 second or more are shown as "Hh Mm Ss" (e.g., "2h 15m 42s")</item></list>
    /// </summary>
    /// <param name="duration">The duration to format.</param>
    /// <returns>A formatted string representing the duration.</returns>
    public static string Format(TimeSpan duration) {
        if (duration.TotalSeconds < 1)
            return $"{duration.TotalMilliseconds:F0} ms";

        int hours = (int)duration.TotalHours;
        int minutes = duration.Minutes;
        int seconds = duration.Seconds;

        return $"{hours}h {minutes}m {seconds}s";
    }

    /// <summary>
    /// Formats a <see cref="DateTime"/> or <see cref="TimeSpan"/> into a string according to the specified <see cref="TimeStyle"/>.
    /// </summary>
    /// <param name="input">
    /// The time value to format. Must be either: <see cref="DateTime"/>,<see cref="TimeSpan"/>, which is mapped onto today’s date for formatting.
    /// </param>
    /// <param name="style"> The output format to apply </param>
    /// <returns>
    /// A formatted time string based on the chosen <paramref name="style"/>.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static string FormatTime(object input, TimeStyle style) {
        DateTime dateTime = input switch {
            TimeSpan ts => DateTime.Today.Add(ts),
            DateTime dt => dt,
            _ => throw new ArgumentException("Input must be either DateTime or TimeSpan."),
        };
        return style switch {
            TimeStyle.TimeOnly => dateTime.ToString("HH:mm:ss"),
            TimeStyle.DateAndTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            TimeStyle.TimeAmPm => dateTime.ToString("hh:mm:ss tt"),
            _ => throw new ArgumentOutOfRangeException(nameof(style), "Unsupported format style.")
        };
    }
}
