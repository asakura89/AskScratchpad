Clear-Host

$user = [System.Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object System.Security.Principal.WindowsPrincipal($user)
$adminRole = [System.Security.Principal.WindowsBuiltInRole]::Administrator
$role = "Not Admin"
If ($principal.IsInRole($adminRole)) {
    $role = "Admin"
}

Write-Host "'$($user.Name)' is $($role)"
