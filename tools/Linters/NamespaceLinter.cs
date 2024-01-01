using System.Text.RegularExpressions;

var directoryPath = Path.GetFullPath(Environment.CurrentDirectory + "/../../src");
var directoriesNamespaces = new Dictionary<string, string>
{
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Grpc/Interop/"), "MaaFramework.Binding.Interop.Grpc" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Native/Interop/"), "MaaFramework.Binding.Interop.Native" },


    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/Abstractions/"), "MaaFramework.Binding.Abstractions" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Grpc/Abstractions/"), "MaaFramework.Binding.Abstractions.Grpc" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Native/Abstractions/"), "MaaFramework.Binding.Abstractions.Native" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/Buffers/"), "MaaFramework.Binding.Buffers" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Grpc/Buffers/"), "MaaFramework.Binding.Buffers" },
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Native/Buffers/"), "MaaFramework.Binding.Buffers" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding/Custom/"), "MaaFramework.Binding.Custom" },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.UnitTests/"), string.Empty },

    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding"), "MaaFramework.Binding" },
};
var protosNamespaces = new Dictionary<string, string>
{
    { Path.GetFullPath($"{directoryPath}/MaaFramework.Binding.Grpc/Protos"), "MaaFramework.Binding.Interop.Grpc" },
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

foreach (var item in protosNamespaces)
{
    foreach (var filePath in Directory.EnumerateFiles(item.Key, "*.proto", SearchOption.AllDirectories))
    {
        var text = File.ReadAllText(filePath);
        if (!text.Contains(item.Value))
        {
            failure = true;
            Console.WriteLine($"::error file={filePath},title=NamespaceLinter::Namespace must be '{item.Value}'");
        }
    }
}

return failure.GetHashCode();
