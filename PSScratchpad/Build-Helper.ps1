Clear-Host

function GetTimeStamp() {
    return [System.DateTime]::Now.ToString("yyyyMMddHHmm")
}
function Log($starting, $message) {
    $timest = $script:timestamp
    If ($timest -Eq $null) {
        $script:timestamp = GetTimeStamp
        $timest = $script:timestamp
    }

    $logFile = "$($script:scriptDir)\build-helper_$($timest).log"
    If ($starting) {
        $message | Out-File -Encoding "UTF8" -FilePath $logFile
    }
    Else {
        $message | Add-Content -Encoding "UTF8" -Path $logfile
    }
}

function CreateMainForm($title) {
    $form = New-Object System.Windows.Forms.Form
    $form.Text = $title
    $form.Width = 600
    $form.Height = 400
    $form.AutoSize = $true
    $form.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen

    return $form
}

function CreateLabel($mainForm, $text, $prevLabel) {
    $label = New-Object System.Windows.Forms.Label
    $label.Font = New-Object System.Drawing.Font("Verdana", [single]10, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $label.Text = $text
    $label.AutoSize = $true
    $x = 5
    $y = 13
    If ($prevLabel -Ne $null) {
        $y = $prevLabel.Location.Y + 30
    }

    $label.Location = New-Object System.Drawing.Point($x, $y)
    return $label
}

function CreateTextBox($mainForm, $prevTextBox) {
    $textbox = New-Object System.Windows.Forms.TextBox
    $textbox.Font = New-Object System.Drawing.Font("Verdana", [single]10, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $textbox.Size = New-Object System.Drawing.Size(450, 24);
    $textbox.AutoSize = $true
    $x = 90
    $y = 10
    If ($prevTextBox -Ne $null) {
        $y = $prevTextBox.Location.Y + 30
    }

    $textbox.Location = New-Object System.Drawing.Point($x, $y)
    return $textbox
}

function CreateButton($mainForm, $text, $action, $prevTextBox) {
    $button = New-Object System.Windows.Forms.Button
    $button.Font = New-Object System.Drawing.Font("Verdana", [single]10, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $button.Text = $text
    $button.Size = New-Object System.Drawing.Size(150, 24);
    $button.AutoSize = $true
    $x = 90
    $y = 10
    If ($prevTextBox -Ne $null) {
        $y = $prevTextBox.Location.Y + 30
    }

    $button.Location = New-Object System.Drawing.Point($x, $y)
    $button.Add_Click($action)
    return $button
}

function GetSlns() {
    $files = Get-ChildItem -Path $txtMainDir.Text -Include "*.sln" -Recurse |
        Select-Object -ExpandProperty VersionInfo  |
        Select-Object -ExpandProperty FileName

    return $files
}

function Build() {
    Log $false "Build"

    $timest = $script:timestamp
    If ($timest -Eq $null) {
        $script:timestamp = GetTimeStamp
        $timest = $script:timestamp
    }
    $logFile = "$($script:scriptDir)\build_$($timest).log"
    $starting = $true
    GetSlns |
        ForEach-Object {
            If ($starting) {
                (dotnet build --verbosity m $_) |
                    Out-File -Encoding "UTF8" -FilePath $logFile

                $starting = $false
            }
            Else {
                (dotnet build --verbosity m $_) |
                    Add-Content -Encoding "UTF8" -Path $logfile
            }
        }
}

function Run() {
    Log $false "Run"

    $projs = @()
    GetSlns |
        ForEach-Object {
            $slnDir = [System.IO.Path]::GetDirectoryName($_)
            $csprojName = (Get-Item $slnDir).Name + ".csproj"
            $projs += Get-ChildItem -Path $slnDir -Include $csprojName -Recurse |
                Select-Object -ExpandProperty VersionInfo  |
                Select-Object -ExpandProperty FileName |
                Select-Object -First 1
            
                Log $false $($projs | ConvertTo-Json)
        }

    $projs |
        ForEach-Object {
            $projDir = [System.IO.Path]::GetDirectoryName($_)
            $arr = $_ -Split "\\" | Where-Object { $_ }
            $cmd = "`$Host.UI.RawUI.WindowTitle=`'$($arr[$arr.Length-2])`'"
            $cmd2 = "-NoExit -Command `"$($cmd); dotnet watch run --no-build`""
            Start-Process $script:pwsh -WorkingDirectory $projDir -ArgumentList $cmd2
        }
}

function IntializeComponent() {
    Log $false "IntializeComponent"

    $script:mainF = CreateMainForm ".Net Core project builder helper"
    $script:lblMainDir = CreateLabel $mainF "Main-Dir"
    $script:txtMainDir = CreateTextBox $mainF
    $script:btnBuild = CreateButton $mainF "Build" ${function:Build} $txtMainDir
    $script:btnRun = CreateButton $mainF "Run" ${function:Run} $txtMainDir
    $script:btnRun.Location = New-Object System.Drawing.Point($($script:btnBuild.Location.X + $script:btnBuild.Width + 5), $script:btnBuild.Location.Y)

    $script:mainF.Controls.Add($lblMainDir)
    $script:mainF.Controls.Add($txtMainDir)
    $script:mainF.Controls.Add($btnBuild)
    $script:mainF.Controls.Add($btnRun)
}

function InitValue() {
    Log $false "InitValue"

    $mainDir = [System.IO.Path]::GetFullPath("$($script:scriptDir)\..\project")
    $txtMainDir.Text = $mainDir
}

Add-Type -Assembly System.Drawing
Add-Type -Assembly System.Windows.Forms

$pwsh = "Powershell"
$scriptDir = $(Split-Path -parent $PSCommandPath)
$timestamp = $null

Log $true "Starting."
IntializeComponent
InitValue
Log $false "Starting Done."

$mainF.ShowDialog()