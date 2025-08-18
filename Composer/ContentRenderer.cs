using System.Text;
using TrganReport.Core;
using TrganReport.Enums;
using TrganReport.Interfaces;
using TrganReport.Models;
using TrganReport.Utils;

namespace TrganReport.Composer;
internal sealed class ContentRenderer : IReportSection {
    static readonly string Image_Css = ReportTemplates.Css_Image;
    static readonly string Step_Css = ReportTemplates.Css_Step;
    static readonly string Log_Css = ReportTemplates.Css_Log;
    static readonly string Test_Css = ReportTemplates.Css_Test;

    static readonly string Image_Js = ReportTemplates.Js_Image;
    static readonly string Log_Js = ReportTemplates.Js_Log;
    static readonly string Test_Js = ReportTemplates.Js_Test;

    static readonly string Image_Html = ReportTemplates.Image;
    static readonly string Log_Html = ReportTemplates.Log;
    static readonly string Step_Html = ReportTemplates.Step;
    static readonly string Test_Html = ReportTemplates.Test;
    static readonly string Container_Html = ReportTemplates.Container;
    public static readonly ContentRenderer Instance = new();
    private ContentRenderer() { }

    private bool containsImage = false;
    private bool containsLogs = false;

    public SectionContent Render(TrganHtmlReport report) {
        string html = GenerateTestContent(report);
        string css = BuildCss();
        string js = BuildJs();
        return new SectionContent { Html = html, Css = css, Js = js };
    }

    private string GenerateTestContent(TrganHtmlReport report) {
        StringBuilder containerBlock = new();
        string defaultContainerHtml = string.Empty;

        System.Collections.Generic.IReadOnlyList<TrganContainer> containers = report.TrganContainers;
        report.ReportStats.TotalTestContainers = containers.Count;
        if (containers.Count == 0) return "";
        if (containers.Count > 1 || containers[0].ContainerDetails.Name != "default")
            containerBlock.AppendLine("<h2>EXECUTION REPORT</h2>");

        foreach (TrganContainer container in containers) {
            StringBuilder testBlock = new();
            report.ReportStats.UpdateStat(StatusType.Container, container.ContainerDetails.Status);
            report.ReportStats.TotalTests += container.Tests.Count;

            foreach (TrganTest test in container.Tests)
                testBlock.Append(RenderTestBlock(test, report));

            string containerHtml = container.ContainerDetails.Name == "default"
                ? RenderDefaultContainer(testBlock.ToString())
                : RenderNamedContainer(report.Config.OfflineMode, container.ContainerDetails, testBlock.ToString());

            if (container.ContainerDetails.Name == "default")
                defaultContainerHtml = containerHtml;
            else
                containerBlock.AppendLine(containerHtml);
        }
        return containerBlock.Append(defaultContainerHtml).ToString();
    }

    private string RenderTestBlock(TrganTest test, TrganHtmlReport report) {
        StringBuilder stepBlock = new();
        report.ReportStats.UpdateStat(StatusType.Test, test.Status);
        report.ReportStats.TotalSteps += test.Steps.Count;

        foreach (TrganStep step in test.Steps)
            stepBlock.AppendLine(RenderStepBlock(step, report));

        return Test_Html
            .Replace("<!--NAME-->", test.testDetails.Name)
            .Replace("/*STATUS*/", test.Status.ToString().ToLower())
            .Replace("/*SRC*/", StatusIcons.GetIcon(report.Config.OfflineMode, test.Status))
            .Replace("<!--STEPS-->", stepBlock.ToString());
    }

    private string RenderStepBlock(TrganStep step, TrganHtmlReport report) {
        StringBuilder logBlock = new();
        report.ReportStats.UpdateStat(StatusType.Step, step.Status);

        if (step.Logs.Count > 0) {
            containsLogs = true;
            foreach (LogEntry log in step.Logs)
                logBlock.Append(RenderLogBlock(log));
        }

        string stepHtml = Step_Html
            .Replace("<!--DESCRIPTION-->", step.Description)
            .Replace("<!--DURATION-->", TimeFormatter.Format(step.EndTime - step.StartTime))
            .Replace("/*STATUS*/", step.Status.ToString().ToLower())
            .Replace("/*SRC*/", StatusIcons.GetIcon(report.Config.OfflineMode, step.Status))
            .Replace("<!--LOGS-->", logBlock.ToString())
            .Replace("<!--ICON-->", StatusIcons.GetTextIcon(step.Status))
            .Replace("<!--KEYWORD-->", step.Keyword != GherkinKeyword.None ? step.Keyword.ToString() : "")
            .Replace("<!--IMG-->", step.ScreenshotId != 0 ? GetModalBlock(step.ScreenshotId, step.ScreenshotTitle) : "");

        if (step.ScreenshotId != 0)
            containsImage = true;

        return stepHtml;
    }

    private static string RenderLogBlock(LogEntry log) {
        string logHtml = Log_Html
            .Replace("<!--TIME-->", log.Timestamp)
            .Replace("<!--LOG-->", log.Message)
            .Replace("<!--LOG_IMG-->", log.ScreenshotId != 0 ? GetModalBlock(log.ScreenshotId, log.ScreenshotTitle) : "");
        return logHtml;
    }

    private static string RenderNamedContainer(bool offlineMode, ExecutionDetails details, string testBlock) =>
        Container_Html
            .Replace("<!--NAME-->", details.Name)
            .Replace("/*STATUS*/", details.Status.ToString().ToLower())
            .Replace("/*SRC*/", StatusIcons.GetIcon(offlineMode, details.Status))
            .Replace("<!--STEPS-->", testBlock);

    private static string RenderDefaultContainer(string testBlock) =>
        @"<h2>TEST REPORT</h2>
        <div class=""test-details"" name=""/*T_NAME*/"" style=""display: block;"">
            <!--STEPS-->
        </div>"
        .Replace("/*T_NAME*/", "default")
        .Replace("<!--STEPS-->", testBlock);

    private string BuildCss() =>
        Test_Css + Step_Css +
        (containsImage ? Image_Css : "") +
        (containsLogs ? Log_Css : "");

    private string BuildJs() =>
        Test_Js +
        (containsImage ? Image_Js : "") +
        (containsLogs ? Log_Js : "");

    private static string GetModalBlock(int id, string title) {
        return Image_Html.Replace("{id}", "" + id).Replace("{TITLE}", title);
    }
}
