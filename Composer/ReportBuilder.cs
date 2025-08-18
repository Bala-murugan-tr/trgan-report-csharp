using System.Collections.Generic;
using System.Text;
using TrganReport.Interfaces;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Responsible for assembling and generating the final HTML report.s
/// </summary>
/// <remarks>
/// This builder orchestrates the rendering of individual report sections, aggregates their CSS, HTML, and JavaScript,
/// and embeds them into a predefined report template. The final output is written to the specified file path.
/// </remarks>
internal class ReportBuilder {
    /// <summary>
    /// Builds the complete HTML report using the provided <see cref="TrganReport"/> data.
    /// </summary>
    public static void Build(TrganHtmlReport trganReport) {
        List<IReportSection> sections = [
            SummaryRenderer.Instance,
            MetaInfoRenderer.Instance,
            StatusTableRenderer.Instance,
            GenerateTrganTable.Instance,
            ContentRenderer.Instance
            ];

        StringBuilder cssBuilder = new();
        StringBuilder htmlBuilder = new();
        StringBuilder jsBuilder = new();

        if (trganReport.Config.OfflineMode) {
            foreach (IReportSection section in sections) {
                Models.SectionContent content = section.Render(trganReport);
                htmlBuilder.AppendLine(content.Html);
                cssBuilder.AppendLine(content.Css);
                jsBuilder.AppendLine(content.Js);
            }
            cssBuilder.Append(ReportTemplates.Css_Body);
        } else {
            foreach (IReportSection section in sections) {
                Models.SectionContent content = section.Render(trganReport);
                htmlBuilder.AppendLine(content.Html);
            }
            string offlineIndicator = "if (!navigator.onLine) {\r\n  alert('You are offline. Some features may not work.');\r\n  document.querySelectorAll('.filter-row').forEach(row => {\r\n    row.style.display = 'none';});}";
            jsBuilder.Append(offlineIndicator);
        }

        jsBuilder.Insert(0, $"{ChartRenderer.ChartData(trganReport.ReportStats)}");
        string finalReport = ReportTemplates.Index
            .Replace("/*css*/", cssBuilder.ToString())
            .Replace("/*js*/", jsBuilder.ToString())
            .Replace("<!--body-->", htmlBuilder.ToString());

        trganReport.Writer.WriteContent(finalReport);
        trganReport.Writer.LoadFooter(trganReport.Config.OfflineMode);
        trganReport.Writer.Close();
    }
}
