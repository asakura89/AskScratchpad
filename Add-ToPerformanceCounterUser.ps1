Clear-Host

$groups = @()
Get-LocalGroup |
    Select-Object -ExpandProperty Name |
    Select-String 'performance' |
    ForEach-Object { $groups += $_ }

$users = @()
Get-IISAppPool |
    Select-Object @{E={"IIS APPPOOL\$($_.Name)"};L="Name"} |
    Select-Object -ExpandProperty Name |
    ForEach-Object { $users += $_ }

ForEach ($group in $groups) {
    ForEach ($user in $users) {
        Add-LocalGroupMember -Group "$($group)" -Member "$($user)"
    }
}

