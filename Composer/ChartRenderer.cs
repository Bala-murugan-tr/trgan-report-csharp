using System.Text;
using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
/// <summary>
/// Renders the chart section of the Trgan report, including chart titles and JavaScript data.
/// </summary>
internal sealed class ChartRenderer : IReportSection {
    static readonly string Css = ReportTemplates.Css_MetaChart;
    static readonly string Js = ReportTemplates.Js_MetaChart;
    static readonly string Html = ReportTemplates.MetaChart;
    /// <summary> Singleton instance of the chart renderer.</summary>
    public static readonly ChartRenderer Instance = new();
    private ChartRenderer() { }
    public SectionContent Render(TrganHtmlReport report) {
        return new SectionContent {
            Css = Css,
            Js = Js,
            Html = UpdateChartNames(report),
        };
    }
    /// <summary>
    /// Injects chart titles into the chart HTML template using report configuration.<para>Refer =>
    /// <see cref="Configs.ReportConfig"/></para>
    /// </summary>
    /// <returns>HTML string with chart titles populated.</returns>
    private static string UpdateChartNames(TrganHtmlReport report) {
        string testTitle = report.Config.TestChartTitle;
        string subTestTitle = report.Config.SubTestChartTitle;
        string stepTitle = report.Config.StepChartTitle;
        //var chartOrder = report.Config.ChartOrder;//not implemented
        return Html.Replace("<!--TRGAN_TEST-->", testTitle)
            .Replace("<!--TRGAN_SUB__TEST-->", subTestTitle)
            .Replace("<!--TRGAN_STEP-->", stepTitle);
    }
    /// <summary>
    /// Builds a JavaScript array data for chart data using pass/fail/skip/total counts.
    /// </summary>
    private static string BuildChartData(string variableName, int pass, int fail, int skip, int total) {
        return
   $@"window.{variableName} = [
      {{label: 'PASS', count: {pass}, color: '#2ecc71' }},
      {{label: 'Fail', count: {fail}, color: '#e74c3c' }},
      {{label: 'Skip', count: {skip}, color: '#099bfd' }},
      {{label: 'Total', count: {total}, color: '#34495e' }}
    ];";
    }
    /// <summary>
    /// Generates JavaScript chart data for tests, sub-tests, and steps using report statistics.
    /// </summary>
    internal static string ChartData(ReportStats stats) {
        StringBuilder sb = new();

        sb.Append(BuildChartData("testSummaryData",
            stats.PassedTestContainers, stats.FailedTestContainers, stats.SkippedTestContainers, stats.TotalTestContainers));
        sb.Append("\n      ");
        sb.Append(BuildChartData("testBlockSummaryData",
            stats.PassedTests, stats.FailedTests, stats.SkippedTests, stats.TotalTests));
        sb.Append("\n      ");
        sb.Append(BuildChartData("stepSummaryData",
            stats.PassedSteps, stats.FailedSteps, stats.SkippedSteps, stats.TotalSteps));

        return sb.ToString();
    }
}
