Clear-Host

Get-LocalGroup |
    Select-Object -ExpandProperty Name |
    Select-String 'performance'
