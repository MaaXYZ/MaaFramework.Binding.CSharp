#!/bin/bash

echo -e "(1) Main\n(2) PI\n(3) Agent\nSelect: "
read -r n
case $n in
    1) path="main.csx" ;;
    2) path="pi_cli.csx" ;;
    3) path="Agent/AgentMain.cs" ;;
    *) echo "Invalid selection."; exit 1 ;;
esac

ScriptRoot=$(dirname "$0")
cd "$ScriptRoot"
export DOTNET_SCRIPT_CACHE_LOCATION="$ScriptRoot/.cache"

dotnet tool restore
dotnet script "$path" --sources https://api.nuget.org/v3/index.json --sources https://maaxyz.github.io/pkg/nuget/index.json