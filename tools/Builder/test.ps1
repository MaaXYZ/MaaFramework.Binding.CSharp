cd ../../

$env:GITHUB_RUN_ID = "11165266892"
$env:GITHUB_WORKFLOW = "ci"
$env:GITHUB_OUTPUT = New-TemporaryFile
$env:GITHUB_STEP_SUMMARY = $env:GITHUB_OUTPUT
dotnet script ./tools/Builder/Build.csx

ls nupkgs
rmdir nupkgs
rm $env:GITHUB_OUTPUT

