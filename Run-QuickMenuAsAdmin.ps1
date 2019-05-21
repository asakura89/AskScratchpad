Clear-Host

$powershellPath = "C:\\PowerShell-6.1.3-win-x64\\pwsh.exe"
$menuList = @(
    @{
        Name = "Visual Studio 2017"
        Value = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\devenv.exe"
    },
    @{
        Name = "Visual Studio Code"
        Value = "C:\\VSCode\\Code.exe"
    },
    @{
        Name = "SSMS 17"
        Value = "C:\\Program Files (x86)\\Microsoft SQL Server\\140\\Tools\\Binn\\ManagementStudio\\Ssms.exe"
    },
    @{
        Name = "Git Extensions"
        Value = "C:\\Program Files (x86)\\GitExtensions\\GitExtensions.exe"
    },
    @{
        Name = "IIS Manager"
        Value = "C:\\Windows\\system32\\inetsrv\\InetMgr.exe"
    },
    @{
        Name = "Task Manager"
        Value = "C:\\Windows\\system32\\Taskmgr.exe"
    },
    @{
        Name = "Explorer"
        Value = "C:\\Windows\\explorer.exe"
    },
    @{
        Name = "Command Prompt"
        Value = "C:\\Windows\\system32\\cmd.exe"
    },
    @{
        Name = "Windows Powershell"
        Value = "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe"
    },
    @{
        Name = "Powershell Core"
        Value = "C:\\PowerShell-6.1.3-win-x64\\pwsh.exe"
    },
    @{
        Name = "System Path"
        Value = "C:\\Windows\\System32\\SystemPropertiesAdvanced.exe"
    },
    @{
        Name = "Certificate Manager CurrentUser"
        Value = "certmgr.msc"
    },
    @{
        Name = "Certificate Manager LocalMachine"
        Value = "certlm.msc"
    },
    @{
        Name = "Event Viewer"
        Value = "eventvwr.msc"
    },
    @{
        Name = "Services"
        Value = "services.msc"
    }
)

Do {
    Write-Host ""
    Write-Host "Run What ?"
    $counter = 1
    ForEach ($app In $menuList) {
        Write-Host "$($counter). $($app.Name)"
        $counter++
    }
    $inp = Read-Host
    [System.Int32] $outval = -1
    $isnum = [System.Int32]::TryParse($inp, [ref] $outval)
    If ($isnum) {
        $app = $menuList[$inp -1]
        If ($app) {
            Write-Host "Running $($app.Name)"
            Start-Process $($app.Value) -Verb RunAs
        }
    }
    ElseIf ($inp.Length -Ge 3) {
        Start-Process $powershellPath -ArgumentList "-Command & $($inp)" -Verb RunAs
    }
}
While ($inp -ne "Q" -and $inp -ne "E" -and $inp -ne "C")

Write-Host "Quitting."
