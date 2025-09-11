using System.Text.RegularExpressions;

var directoryPath = Path.GetFullPath(Environment.CurrentDirectory + "/../../src");
var directoriesNamespaces = new Dictionary<string, string>
{
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Native/Interop/"), "MaaFramework.Binding.Interop.Native" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/Abstractions/"), "MaaFramework.Binding.Abstractions" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Native/Abstractions/"), "MaaFramework.Binding.Abstractions" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/Buffers/"), "MaaFramework.Binding.Buffers" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Native/Buffers/"), "MaaFramework.Binding.Buffers" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/Custom/"), "MaaFramework.Binding.Custom" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/MaaMsg.cs"), "MaaFramework.Binding.Notification" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Extensions/Notification"), "MaaFramework.Binding.Notification" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.UnitTests/"), string.Empty },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding"), "MaaFramework.Binding" },
};

var failure = false;
foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*.cs", SearchOption.AllDirectories))
{
    if (new List<string> { @"\obj\", "/obj", @"\bin\", "bin" }.Exists(filePath.Contains))
        continue;

    var matchPath = directoriesNamespaces.Keys.FirstOrDefault(filePath.Contains);
    if (matchPath is null
        || !directoriesNamespaces.TryGetValue(matchPath, out var matchNamespace)
        || string.IsNullOrEmpty(matchNamespace))
    {
        Console.WriteLine($"::notice file={filePath},title=NamespaceLinter::Skip file");
        continue;
    }

    var text = File.ReadAllText(filePath);
    var namespacePattern = $@"(?<=namespace\s){Regex.Escape(matchNamespace)}(?=\s*(\{{|;))";
    var match = Regex.Match(text, namespacePattern);

    if (!match.Success)
    {
        failure = true;
        Console.WriteLine($"::error file={filePath},title=NamespaceLinter::Namespace must be '{matchNamespace}'");
    }
}

return failure.GetHashCode();
