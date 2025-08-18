namespace TrganReport.Utils;
internal static class AssetPath {
    //private const string RepoVersion = "v1.0";
    private const string RepoVersion = "3d9dd35f999b98f85c6e913a6435311d19e2436e";
    private const string AppVersion = "v1.0";
    internal static string CdnBaseUrl = $"https://cdn.jsdelivr.net/gh/trgan-reports/trgan-report-cdn@{RepoVersion}/{AppVersion}";


    internal static readonly string StyleCssUrl = $"{CdnBaseUrl}/style.css";
    internal static readonly string ScriptJsUrl = $"{CdnBaseUrl}/script.js";

    private static readonly string IconsUrl = $"{CdnBaseUrl}/icons";
    internal static readonly string FaviconsUrl = $"{CdnBaseUrl}/logo";

    internal static readonly string PassIconUrl = $"{IconsUrl}/pass.png";
    internal static readonly string FailIconUrl = $"{IconsUrl}/fail.png";
    internal static readonly string SkipIconUrl = $"{IconsUrl}/skip.png";
}
