$ENV:DOTNET_SCRIPT_CACHE_LOCATION="$PSScriptRoot\.cache"
dotnet tool restore
dotnet script AgentMain.cs -s https://api.nuget.org/v3/index.json -s https://maaxyz.github.io/pkg/nuget/index.json