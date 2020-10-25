Clear-Host

Get-IISSite |
    Select-Object -ExpandProperty "Name"

Get-IISSite |
    Select-Object Id, Applications, Bindings, Name, State


Import-Module WebAdministration
Get-ChildItem IIS:\Sites


Import-Module WebAdministration
Get-ChildItem IIS:\Sites |
    Select-Object Id, Bindings, Name, State

