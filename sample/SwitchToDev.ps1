<#
.SYNOPSIS
    为所有 .csx 文件添加 DLL 引用
.DESCRIPTION
    本脚本会在当前目录及子目录的所有 .csx 文件开头插入指定的 DLL 引用
.EXAMPLE
    .\Add-DllReferences.ps1
#>

# 需要添加的 DLL 引用（请根据实际项目结构调整路径）
$references = @(
    '#r "..\..\src\MaaFramework.Binding.UnitTests\bin\Debug\net9.0\MaaFramework.Binding.Native.dll"',
    '#r "..\..\src\MaaFramework.Binding.UnitTests\bin\Debug\net9.0\MaaFramework.Binding.dll"',
    '#r "..\..\src\MaaFramework.Binding.UnitTests\bin\Debug\net9.0\MaaFramework.Binding.Extensions.dll"',
    '#r "nuget: Maa.Framework.Runtime.win-x64, *"'
    '#nullable enable'
)

# 获取当前目录及子目录中所有 .csx 文件
$csxFiles = Get-ChildItem -Path . -Filter "*.csx" -Recurse -File

foreach ($file in $csxFiles) {
    # 读取文件内容
    $content = Get-Content -Path $file.FullName -Raw

    # 移除前4行
    $lines = $content -split "`r?`n"
    if ($lines.Count -gt 4) {
        $contentWithoutFirst4Lines = ($lines[4..($lines.Count - 1)] -join "`r`n")
    } else {
        $contentWithoutFirst4Lines = ""
    }

    # 检查是否已存在这些引用（如果存在则跳过）
    $alreadyExists = $references | ForEach-Object { $content -match [regex]::Escape($_) } | Where-Object { $_ }
    if ($alreadyExists.Count -eq $references.Count) {
        Write-Host "引用已存在于 $($file.FullName) - 跳过"
        continue
    }

    # 将新引用插入到文件开头
    $newContent = ($references -join "`r`n") + "`r`n`r`n" + $contentWithoutFirst4Lines

    # 写回文件
    Set-Content -Path $file.FullName -Value $newContent -NoNewline

    Write-Host "已为 $($file.FullName) 添加引用"
}

Write-Host "操作完成"
Read-Host
