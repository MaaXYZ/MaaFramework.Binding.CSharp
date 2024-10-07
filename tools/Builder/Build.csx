#r "nuget: NuGet.Versioning, 6.11.0"

using System.Diagnostics;
using NuGet.Versioning;

var GITHUB_OUTPUT = Environment.GetEnvironmentVariable("GITHUB_OUTPUT")
    ?? throw new InvalidOperationException("$GITHUB_OUTPUT is null.");
var GITHUB_RUN_ID = Environment.GetEnvironmentVariable("GITHUB_RUN_ID")
    ?? throw new InvalidOperationException("$GITHUB_RUN_ID is null.");
var GITHUB_WORKFLOW = Environment.GetEnvironmentVariable("GITHUB_WORKFLOW")
    ?? throw new InvalidOperationException("GITHUB_WORKFLOW is null.");
var GITHUB_REF = Environment.GetEnvironmentVariable("GITHUB_REF")
    ?? string.Empty;
var d = DateTimeOffset.Parse(StartProcess($"gh run view {GITHUB_RUN_ID} --json createdAt --jq .createdAt")).ToOffset(TimeSpan.FromHours(8));

var dateTime = ((d.Year - 2000) * 1000 + d.Month * 50 + d.Day).ToString("D5");
var todayBuildTimes = StartProcess($"gh run list --workflow {GITHUB_WORKFLOW} --created {d.ToString("yyyy-MM-ddT00:00:00+08:00")}..{d.ToString("yyyy-MM-ddT23:59:59+08:00")} --limit 99 --json createdAt --jq length");
var defaultBranch = StartProcess("gh repo view --json defaultBranchRef --jq .defaultBranchRef.name");
var branch = StartProcess("git rev-parse --abbrev-ref HEAD");
var commit = StartProcess("git rev-parse HEAD");
var isRelease = GITHUB_REF.StartsWith("refs/tags/v");
var tag = StartProcess($"git describe --tags --match v*");
var tags = new List<string>(tag.TrimStart('v').Split('-'));
var version = tags.Count switch
{
    1 or 3 => NuGetVersion.Parse(       // => v2.0.1
        string.Join('-', tags[..1])),   //    v2.0.1      v2.0.1-3-ge878f0b
    2 or 4 => NuGetVersion.Parse(       // => v2.0.1-rc.1
        string.Join('-', tags[..2])),   //    v2.0.1-rc.1 v2.0.1-rc.1-3-ge878f0b
    _ => throw new InvalidOperationException("The release labels count > 4."),
};

if (tags.Count is 3 or 4)               // 非最新版本号
{
    version = new NuGetVersion(version.Major, version.Minor, version.Patch,
        ["preview", dateTime, todayBuildTimes],
        tag);
}

var verStr = version.ToFullString();
TeeToGithubOutput(
    $"tag={tag}",
    $"version={verStr}",
    $"is_release={isRelease}",
    $"default_branch={defaultBranch}"
    );
StartProcess(
    redirectStandardOutputToReturn: false,
    cmd: $"dotnet build --configuration Release --no-restore -p:Version={verStr};RepositoryBranch={branch};RepositoryCommit={commit};{(isRelease ? string.Empty : "DebugType=embedded;IncludeSymbols=false")}"
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
string StartProcess(string cmd, bool redirectStandardOutputToReturn = true)
{
    Console.WriteLine(">> {0}", cmd);
    var cmds = cmd.Split(' ', 2, StringSplitOptions.TrimEntries);
    using var p = Process.Start(new ProcessStartInfo
    {
        FileName = cmds[0],
        Arguments = cmds[1],
        RedirectStandardOutput = redirectStandardOutputToReturn,
    });
    p.WaitForExit();
    if (p.ExitCode != 0)
        throw new InvalidOperationException($"ExitCode is {p.ExitCode} from {cmd}.");
    var output = redirectStandardOutputToReturn ? p.StandardOutput.ReadToEnd().Trim() : string.Empty;
    Console.WriteLine(output);
    return output;
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
