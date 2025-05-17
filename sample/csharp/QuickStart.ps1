$n = [int](Read-Host "(1) Main`n(2) PI`n(3) Agent`nSelect")
$path = @('main.csx', 'pi_cli.csx', 'Agent/AgentMain.cs')[$n - 1]
if ($n -lt 1 -or $n -gt 3) {
    Write-Error "Invalid selection."
    exit 1
}

cd "$PSScriptRoot"
$ENV:DOTNET_SCRIPT_CACHE_LOCATION = "$PSScriptRoot\.cache"

dotnet tool restore
dotnet script "$path" --sources https://api.nuget.org/v3/index.json --sources https://maaxyz.github.io/pkg/nuget/index.json