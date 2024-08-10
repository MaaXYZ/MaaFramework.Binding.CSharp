#r "nuget: NuGet.Versioning, 6.10.1"

using System.Diagnostics;
using NuGet.Versioning;

var GITHUB_OUTPUT = Environment.GetEnvironmentVariable("GITHUB_OUTPUT")
    ?? throw new InvalidOperationException("$GITHUB_OUTPUT is null.");

var branch = StartProcess("git rev-parse --abbrev-ref HEAD", true);
var commit = StartProcess("git rev-parse HEAD", true);
var gitRef = Environment.GetEnvironmentVariable("GITHUB_REF") ?? string.Empty;
var isRelease = gitRef.StartsWith("refs/tags/v");
var tag = StartProcess($"git describe --tags --match v* {gitRef}", true); // tag=v1.8.9.2-3-ge878f0b
var tags = new Queue<string>(tag.Split('-'));
var version = NuGetVersion.Parse(tags.Dequeue().TrimStart('v'));

if (version.ReleaseLabels.Count() > 1)
{
    throw new InvalidOperationException("The release labels count > 1.");
}
if (!isRelease)
{
    var nightlyVersion = new NuGetVersion(version.Major, version.Minor, version.Patch,
        version.IsPrerelease
            ? version.Revision                      // 预发布版本号已经提升了一位
            : version.Revision + 1,
        version.IsPrerelease
            ? version.ReleaseLabels.Concat(tags)    // alpha1 -> alpha1.1
            : tags.Prepend("alpha"),                 //        -> alpha.1
        string.Empty
    );
    version = nightlyVersion;
}

var verStr = version.ToFullString();
TeeToGithubOutput(
    $"tag={tag}",
    $"version={verStr}",
    $"is_release={isRelease}"
    );
StartProcess(
    $"dotnet build --configuration Release --no-restore -p:Version={verStr};RepositoryBranch={branch};RepositoryCommit={commit}"
    );
MoveNupkgFiles("nupkgs");


#region Methods
void TeeToGithubOutput(params string[] outputs)
{
    foreach (var output in outputs)
    {
        Console.WriteLine(output);
    }
    File.AppendAllLines(GITHUB_OUTPUT, outputs);
}
string StartProcess(string cmd, bool redirectStandardOutputToReturn = false)
{
    var cmds = cmd.Split(' ', 2, StringSplitOptions.TrimEntries);
    using var p = Process.Start(new ProcessStartInfo
    {
        FileName = cmds[0],
        Arguments = cmds[1],
        RedirectStandardOutput = redirectStandardOutputToReturn,
    });
    p.WaitForExit();
    return redirectStandardOutputToReturn
        ? p.StandardOutput.ReadToEnd().Trim()
        : string.Empty;
}
void MoveNupkgFiles(string destDir)
{
    Directory.CreateDirectory(destDir);
    var files = Directory.GetFiles("src", "*nupkg", SearchOption.AllDirectories);
    foreach (var file in files)
    {
        var fileName = Path.GetFileName(file);
        var destFile = Path.Combine(destDir, fileName);
        File.Move(file, destFile);
        Console.WriteLine($"Moved {file} to {destFile}");
    }
}
#endregion
