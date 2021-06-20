Clear-Host

$scriptDir = $(Split-Path -parent $PSCommandPath)
$dir = "$($scriptDir)\..\project"
$migrateLog = "$($scriptDir)\..\migrate.log"
$buildLog = "$($scriptDir)\..\build.log"

$slnExt = ".sln"
$slnFiles = Get-ChildItem -Path $dir -Include "*$($slnExt)" -Recurse |
    Select-Object -ExpandProperty VersionInfo  |
    Select-Object -ExpandProperty FileName

Write-Host "Start Migrating."

$projs = @()
$slnFiles |
    ForEach-Object {
        $slnDir = [System.IO.Path]::GetDirectoryName($_)
        $csprojName = (Get-Item $slnDir).Name + ".csproj"
        $projs += Get-ChildItem -Path $slnDir -Include $csprojName -Recurse |
            Select-Object -ExpandProperty VersionInfo  |
            Select-Object -ExpandProperty FileName |
            Select-Object -First 1
    }

$dbctxOutput = ""
$dbctxs = @()
$projs |
    ForEach-Object {
        $projDir = [System.IO.Path]::GetDirectoryName($_)
        $result = (dotnet ef dbcontext list --no-build --project $projDir) | Out-String
        $ctxs = $result -Split "\r?\n" | Where-Object { [System.String]::IsNullOrEmpty($_) -Ne $true -And [System.Text.RegularExpressions.Regex]::Match($_, ".+DbContext$").Success }
        $dbctxs += @{ ProjDir=$projDir; Proj=$_; Context=$ctxs; }
    }

$dbctxs | ConvertTo-Json

<#
$dbctxs |
    ForEach-Object {
        $result = (dotnet ef migrations list --no-build --context $_.Context) | Out-String

    }
#>

<#
$starting = $true
$dbctxs |
    ForEach-Object {
        Set-Location $_.ProjDir

        If ($starting) {
            $_.Context |
                ForEach-Object {
                    (dotnet ef database update --no-build --context $_) |
                        Out-File -Encoding "UTF8" -FilePath "$($scriptDir)\..\migrate.log"
                }

            $starting = $false
        }
        Else {
            $_.Context |
                ForEach-Object {
                    (dotnet ef database update --no-build --context $_) |
                        Add-Content -Encoding "UTF8" -Path "$($scriptDir)\..\migrate.log"
                }
        }

        Set-Location $scriptDir
    }
#>

Write-Host "Done."
