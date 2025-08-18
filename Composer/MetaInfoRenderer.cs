using System.Text;
using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Renders the Meta info panel including metainfo line, charts section of the Trgan report.
/// Displays aggregated test statistics including total count, pass/fail/skip breakdown,
/// and overall duration. Uses predefined HTML and CSS templates.
/// </summary>
internal sealed class MetaInfoRenderer : IReportSection {
    // Static templates for styling and layout
    static readonly string Css = ReportTemplates.Css_MetaSummary;
    static readonly string Js = ReportTemplates.Js_MetaSummary;
    static readonly string Html = ReportTemplates.MetaSummary;
    /// <summary>
    /// Singleton instance of the renderer.
    /// </summary>
    public static readonly MetaInfoRenderer Instance = new();
    // Private constructor to enforce singleton usage
    MetaInfoRenderer() { }
    public SectionContent Render(TrganHtmlReport report) {
        SectionContent inline = RenderInline(report);
        SectionContent chart = RenderChart(report);
        string html = Html
            .Replace("<!--META_INFO_INLINE-->", inline.Html)
            .Replace("<!--META_INFO_LINES-->", RenderMetaLines(report))
            .Replace("<!--META_INFO_CHARTS-->", chart.Html);
        string css = Css + chart.Css + inline.Css;
        string js = Js + chart.Js;
        return new SectionContent {
            Css = css,
            Js = js,
            Html = html
        };
    }

    /// <returns>Returns the <see cref="SectionContent"/> for <see cref="ChartRenderer"/></returns>
    private static SectionContent RenderChart(TrganHtmlReport report) {
        return ChartRenderer.Instance.Render(report);
    }
    /// <returns>Returns the <see cref="SectionContent"/> for <see cref="MetaInfoInlineRenderer"/></returns>
    private static SectionContent RenderInline(TrganHtmlReport report) {
        return MetaInfoInlineRenderer.Instance.Render(report);
    }
    /// <summary>
    /// Renders metadata information from the report into styled HTML lines.
    /// Includes standard fields such as executor, date, environment, release version, commit hash, and mode.
    /// Also appends any custom metadata fields provided in the report.
    /// </summary>
    /// <param name="report">The report containing metadata and custom fields.</param>
    /// <returns>HTML string with formatted metadata entries.</returns>
    private static string RenderMetaLines(TrganHtmlReport report) {
        MetaInfo meta = report.MetaInfo;
        StringBuilder sb = new();
        string cssClass = "meta-info";
        void AppendIfNotEmpty(string label, string value, string icon) {
            if (!string.IsNullOrWhiteSpace(value)) {
                icon = report.Config.UseEmojis ? icon : "";
                sb.Append($"<p class={cssClass}>{icon}<strong> {label} : </strong>{value} </p>");
            }
        }
        string executor = !string.IsNullOrWhiteSpace(meta.Executor) ? meta.Executor : "";
        string mode = !string.IsNullOrWhiteSpace(meta.Mode) ? meta.Mode : "";
        string environment = !string.IsNullOrWhiteSpace(meta.Environment) ? meta.Environment : "";
        string date = !string.IsNullOrWhiteSpace(meta.Date) ? meta.Date : "";
        string releaseVersion = !string.IsNullOrWhiteSpace(meta.ReleaseVersion) ? meta.ReleaseVersion : "";
        string sprint = !string.IsNullOrWhiteSpace(meta.Sprint) ? meta.Sprint : "";
        AppendIfNotEmpty("Executed by", executor, "🧑");
        AppendIfNotEmpty("Date", date, "📅");
        AppendIfNotEmpty("Environment", environment, "🌐");
        AppendIfNotEmpty("Release Version", releaseVersion, "🏷️");
        AppendIfNotEmpty("Sprint", sprint, "🔄");
        AppendIfNotEmpty("Commit Hash", meta.CommitHash, "🔗");
        AppendIfNotEmpty("Mode", mode, "🛠️");
        // Custom fields
        string icon = report.Config.UseEmojis ? "\U0001f9e9" : "";
        foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in meta.CustomFields) {
            if (!string.IsNullOrWhiteSpace(kvp.Value)) {
                sb.Append('\n');
                sb.Append($"<p class='{cssClass}'>{icon} <strong>{kvp.Key} : </strong>{kvp.Value}</p>");
            }
        }
        string changeLog = !string.IsNullOrWhiteSpace(meta.ChangeLogSummary) ? meta.ChangeLogSummary : "";
        AppendIfNotEmpty("Change Log", changeLog, "📝");
        return sb.ToString();
    }
}
