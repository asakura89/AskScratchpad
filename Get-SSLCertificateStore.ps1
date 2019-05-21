Clear-Host

# Personal
Get-ChildItem Cert:\LocalMachine\My -Recurse
Get-ChildItem Microsoft.PowerShell.Security\Certificate::LocalMachine\My -Recurse

# Trsuted Root
Get-ChildItem Cert:\LocalMachine\Root -Recurse
Get-ChildItem Microsoft.PowerShell.Security\Certificate::LocalMachine\Root -Recurse

# Intermediate
Get-ChildItem Cert:\LocalMachine\CA -Recurse
Get-ChildItem Microsoft.PowerShell.Security\Certificate::LocalMachine\CA -Recurse

# All
Get-ChildItem Cert:\LocalMachine -Recurse
Get-ChildItem Microsoft.PowerShell.Security\Certificate::LocalMachine -Recurse
