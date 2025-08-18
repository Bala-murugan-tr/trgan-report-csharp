using TrganReport.Models;

namespace TrganReport.Interfaces;
/// <summary>
/// Defines a contract for rendering a modular section of a Trgan report.
/// Implementations are responsible for generating HTML, CSS, and JavaScript
/// fragments that contribute to the final report output.
/// </summary>
internal interface IReportSection {
    /// <summary>
    /// Renders the section content based on the provided report context.
    /// </summary>
    /// <param name="report">The report instance containing data and configuration.</param>
    /// <returns>
    /// A <see cref="SectionContent"/> object containing the HTML, CSS, and JS
    /// fragments for this section.
    /// </returns>
    SectionContent Render(TrganHtmlReport report);
}

