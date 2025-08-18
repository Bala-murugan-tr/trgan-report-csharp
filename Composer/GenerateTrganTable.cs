using System.Text;
using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Renders the Trgan execution outcome table section, displaying dynamic rows and columns.
/// </summary>
internal sealed class GenerateTrganTable : IReportSection {
    private readonly string TableCss = ReportTemplates.Css_TrganTable;
    private readonly string TableJs = ReportTemplates.Js_TrganTable;
    public static readonly GenerateTrganTable Instance = new();
    private GenerateTrganTable() { }
    SectionContent IReportSection.Render(TrganHtmlReport report) {
        if (!report.useTrganTable) return new SectionContent();
        return new SectionContent {
            Css = TableCss,
            Js = TableJs,
            Html = Generate(report)
        };
    }
    /// <summary>
    /// Generates the HTML markup for the Trgan table, including headers and rows.
    /// </summary>
    /// <param name="report">The report containing table data.</param>
    /// <returns>HTML string representing the Trgan table.</returns>
    internal static string Generate(TrganHtmlReport report) {
        Table.TrganTable table = report.GetTrganTable();
        table.FinalizeEntries();
        StringBuilder sb = new();
        sb.AppendLine("<h2>EXECUTION OUTCOME</h2>");
        sb.AppendLine("<div class=\"table-container\">");
        sb.AppendLine("<table class=\"trgan-table\" id=\"trgan-report\">");
        // Header
        sb.AppendLine("<thead><tr>");
        foreach (string col in table.Columns)
            sb.AppendLine($"<th>{System.Net.WebUtility.HtmlEncode(col)}<div class=\"resizer\"></div></th>");
        sb.AppendLine("</tr></thead>");
        // Body
        sb.AppendLine("<tbody>");
        foreach (System.Collections.Generic.Dictionary<string, string> row in table.Rows) {
            sb.AppendLine("<tr>");
            foreach (string col in table.Columns) {
                string cell = row.TryGetValue(col, out string val) ? val : "";
                sb.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(cell)}</td>");
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");
        sb.AppendLine("</div>");
        return sb.ToString();
    }

}
