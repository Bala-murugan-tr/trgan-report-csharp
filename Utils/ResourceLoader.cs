using System.IO;
using System.Reflection;

namespace TrganReport.Utils;
internal static class ResourceLoader {
    public static string Load(string resourceName) {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}


