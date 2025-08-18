namespace TrganReport.Models;
/// <summary>
/// Represents the rendered output of a report section, including HTML, CSS, and JavaScript fragments.
/// </summary>
internal sealed class SectionContent {
    /// <summary>
    /// The HTML markup for the section.
    /// </summary>
    internal string Html { get; set; } = string.Empty;

    /// <summary>
    /// The CSS styles specific to the section.
    /// </summary>
    internal string Css { get; set; } = string.Empty;

    /// <summary>
    /// The JavaScript logic or data required by the section.
    /// </summary>
    internal string Js { get; set; } = string.Empty;
}

