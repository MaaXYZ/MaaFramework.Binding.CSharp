$n = [int](Read-Host "(1) Main`n(2) Agent`n(3) CSBinding README Example`nSelect")
$path = @('1.main.cs', '2.AgentMain.cs', "3.CSBinding README Example.cs")[$n - 1]
if ($n -lt 1 -or $n -gt 4) {
    Write-Error "Invalid selection."
    exit 1
}

cd "$PSScriptRoot"
dotnet run "$path"
