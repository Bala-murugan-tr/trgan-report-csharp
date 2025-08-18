using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Renders the Meta infoline section of the Trgan report.
/// Displays aggregated test statistics including total count, pass/fail/skip breakdown,
/// and overall duration. Uses predefined HTML and CSS templates.
/// </summary>
internal sealed class MetaInfoInlineRenderer : IReportSection {
    // Static templates for styling and layout
    static readonly string Css = ReportTemplates.Css_MetaInline;
    static readonly string Html = ReportTemplates.MetaInline;
    /// <summary>
    /// Singleton instance of the renderer.
    /// </summary>
    public static readonly MetaInfoInlineRenderer Instance = new();
    // Private constructor to enforce singleton usage
    private MetaInfoInlineRenderer() { }
    public SectionContent Render(TrganHtmlReport report) {
        return new SectionContent {
            Css = Css,
            Js = null,
            Html = UpdateInLineMeta(report)
        };
    }
    private static string UpdateInLineMeta(TrganHtmlReport report) {
        MetaInfo meta = report.MetaInfo;
        string sprint = meta.Sprint;
        string release = meta.ReleaseVersion;
        string html = Html
              .Replace("<!--EXECUTOR-->", meta.Executor)
              .Replace("<!--MODE-->", meta.Mode)
              .Replace("<!--ENVIRONMENT-->", meta.Environment)
              .Replace("<!--DATE-->", meta.Date);
        // sprint has higher preference in report
        if (!string.IsNullOrEmpty(sprint))
            html = html.Replace("<!--SPRINT-->", sprint);
        else if (!string.IsNullOrEmpty(release))
            html = html.Replace("Sprint", "Release").Replace("<!--SPRINT-->", release);
        return html;
    }
}
