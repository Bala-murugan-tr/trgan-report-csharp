using TrganReport.Enums;
using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Renders the summary panel section of the Trgan report.
/// Displays aggregated test statistics including total count, pass/fail/skip breakdown,
/// and overall duration. Uses predefined HTML and CSS templates.
/// </summary>
internal sealed class SummaryRenderer : IReportSection {
    // Static templates for styling and layout
    static readonly string Css = ReportTemplates.Css_SummaryPanel;
    static readonly string Html = ReportTemplates.SummaryPanel;
    /// <summary>
    /// Singleton instance of the renderer.
    /// </summary>
    public static readonly SummaryRenderer Instance = new();
    // Private constructor to enforce singleton usage
    SummaryRenderer() { }
    public SectionContent Render(TrganHtmlReport report) {
        return new SectionContent {
            Css = Css,
            Js = null,
            Html = UpdateResultStats(report)
        };
    }
    /// <summary>
    /// Updates the summary HTML template with aggregated test statistics.
    /// If the report contains only a default container, statistics are calculated at the test level.
    /// Otherwise, statistics are derived from container-level statuses.
    /// 
    /// If both containers and tests (will belong to the default container) are present,
    /// the summary will reflect container-level statistics, assuming all open tests are part of the default container.
    /// </summary>
    /// <param name="report">The report containing test and container data.</param>
    /// <returns>Populated HTML string with injected test metrics.</returns>
    private static string UpdateResultStats(TrganHtmlReport report) {
        int total = report.TrganContainers.Count;
        int pass = 0;
        int fail = 0;
        int skip = 0;
        if (total == 0)
            return Html.Replace("<!--TITLE-->", report.Config.ReportHeading)
            .Replace("<!--TOTAL-->", "" + total)
             .Replace("<!--DURATION-->", "" + report.ReportDuration)
             .Replace("<!--PASS-->", "" + pass)
             .Replace("<!--FAIL-->", "" + fail)
             .Replace("<!--SKIP-->", "" + skip);
        if (total == 1 & report.TrganContainers[0].ContainerDetails.Name == "default") {
            total = report.TrganContainers[0].Tests.Count;
            foreach (TrganTest test in report.TrganContainers[0].Tests) {
                if (test.testDetails.Status == Status.PASS) pass++;
                else if (test.testDetails.Status == Status.FAIL) fail++;
                else if (test.testDetails.Status == Status.SKIP) skip++;
            }
        } else {
            foreach (TrganContainer item in report.TrganContainers) {
                if (item.ContainerDetails.Status == Status.PASS) pass++;
                else if (item.ContainerDetails.Status == Status.FAIL) fail++;
                else if (item.ContainerDetails.Status == Status.SKIP) skip++;
            }
        }
        return Html.Replace("<!--TITLE-->", report.Config.ReportHeading)
            .Replace("<!--TOTAL-->", "" + total)
             .Replace("<!--DURATION-->", "" + report.ReportDuration)
             .Replace("<!--PASS-->", "" + pass)
             .Replace("<!--FAIL-->", "" + fail)
             .Replace("<!--SKIP-->", "" + skip);
    }
}
