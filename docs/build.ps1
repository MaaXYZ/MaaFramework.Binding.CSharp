cd $PSScriptRoot
cp -Force ../README.md index.md

$fileContent = Get-Content -Path index.md
$fileContent = $fileContent.Replace("](./", "](https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/tree/main/")
$fileContent = $fileContent.Replace("https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/tree/main/docs/", "./")
$fileContent | Set-Content -Path index.md

if ($env:GITHUB_ACTIONS) {
    docfx
    7z a docs.zip _site\
}
else {
    docfx --serve --open-browser
}
