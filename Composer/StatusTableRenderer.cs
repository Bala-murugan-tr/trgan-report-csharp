using System.Text;
using System.Text.RegularExpressions;
using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Renders the status table section of the Trgan report, showing execution details for containers and tests.
/// </summary>
internal sealed class StatusTableRenderer : IReportSection {
    static readonly string Css = ReportTemplates.Css_StatusTable;
    static readonly string Js = ReportTemplates.Js_StatusTable;
    static readonly string Html = ReportTemplates.StatusTable;
    /// <summary>
    /// Singleton instance of the status table renderer.
    /// </summary>
    public static readonly StatusTableRenderer Instance = new();
    private StatusTableRenderer() { }
    public SectionContent Render(TrganHtmlReport report) {
        return new SectionContent {
            Css = Css,
            Js = Js,
            Html = GenerateStatusTable(report)
        };
    }
    /// <summary>
    /// Generates the HTML markup for the status table, dynamically including columns based on configuration.
    /// </summary>
    /// <param name="report">The report containing containers and test execution details.</param>
    /// <returns>HTML string representing the status table.</returns>
    private static string GenerateStatusTable(TrganHtmlReport report) {
        bool addStartTime = true;
        bool addCategory = report.Config.ShowCategory;
        bool addEndTime = report.Config.ShowEndTime;
        bool addDuration = report.Config.ShowDuration;
        System.Collections.Generic.IReadOnlyList<TrganContainer> containers = report.TrganContainers;
        StringBuilder statustable = new();
        StringBuilder testStatusTable = new();
        foreach (TrganContainer container in containers) {
            ExecutionDetails sts = container.ContainerDetails;
            if (sts.Name == "default") {
                //sts = container
                foreach (TrganTest test in container.Tests) {
                    ExecutionDetails dtails = test.testDetails;
                    testStatusTable.Append($"<tr> <td class='test'> {dtails.Name}</td>");
                    if (addCategory) testStatusTable.Append($"<td>{string.Join("\n", dtails.Category)}</td>");
                    if (addStartTime) testStatusTable.Append($"<td>{TimeFormatter.FormatTime(dtails.StartTime, report.Config.TimeStyle)}</td>");
                    if (addEndTime) testStatusTable.Append($"<td>{TimeFormatter.FormatTime(dtails.EndTime, report.Config.TimeStyle)}</td>");
                    if (addDuration) testStatusTable.Append($"<td>{TimeFormatter.Format(dtails.EndTime - dtails.StartTime)}</td>");
                    testStatusTable.Append($"<td class='{dtails.Status.ToString().ToLower()}'>{dtails.Status}</td></tr>");
                }
                continue;
            }
            statustable.Append($"<tr> <td> {sts.Name}</td>");
            if (addCategory) statustable.Append($"<td>{string.Join(", ", sts.Category)}</td>");
            if (addStartTime) statustable.Append($"<td>{TimeFormatter.FormatTime(sts.StartTime, report.Config.TimeStyle)}</td>");
            if (addEndTime) statustable.Append($"<td>{TimeFormatter.FormatTime(sts.EndTime, report.Config.TimeStyle)}</td>");
            if (addDuration) statustable.Append($"<td>{TimeFormatter.Format(sts.EndTime - sts.StartTime)}</td>");
            statustable.Append($"<td class='{sts.Status.ToString().ToLower()}'>{sts.Status}</td></tr>");
        }
        string html = Html;
        if (!addCategory)
            html = RemoveColumnHeader(html, "Category");
        if (!addStartTime)
            html = RemoveColumnHeader(html, "Start Time");
        if (!addEndTime)
            html = RemoveColumnHeader(html, "End Time");
        if (!addDuration)
            html = RemoveColumnHeader(html, "Duration");
        html = html.Replace("<!--STATUS_TABLE-->", statustable.ToString() + testStatusTable.ToString());
        return html;
    }
    /// <summary>
    /// Removes a column header from the HTML template based on the column name.
    /// </summary>
    /// <param name="html">The HTML string containing the table header.</param>
    /// <param name="columnName">The name of the column to remove.</param>
    /// <returns>Modified HTML string with the specified column header removed.</returns>
    private static string RemoveColumnHeader(string html, string columnName) {
        string pattern = $@"<th.*({columnName}).*</th>";
        return Regex.Replace(html, pattern, "");
    }

}
