Clear-Host

New-Variable -Name ROFeigenbaumConst -Value 46692016 -Option ReadOnly
Write-Host $ROFeigenbaumConst

Set-Variable -Name ROFeigenbaumConst -Value 31415
Write-Host $ROFeigenbaumConst

$ROFeigenbaumConst = 31415
Write-Host $ROFeigenbaumConst

Set-Variable -Name ROFeigenbaumConst -Value 31415 -Force
Write-Host $ROFeigenbaumConst

Set-Variable -Name ROFeigenbaumConst -Value 46692016 -Force
Write-Host $ROFeigenbaumConst

Remove-Variable -Name ROFeigenbaumConst
Write-Host $ROFeigenbaumConst

Remove-Variable -Name ROFeigenbaumConst -Force
Write-Host $ROFeigenbaumConst

New-Variable -Name CONFeigenbaumConst -Value 46692016 -Option Constant
Write-Host $CONFeigenbaumConst

Set-Variable -Name CONFeigenbaumConst -Value 31415
Write-Host $CONFeigenbaumConst

$CONFeigenbaumConst = 31415
Write-Host $CONFeigenbaumConst

Set-Variable -Name CONFeigenbaumConst -Value 31415 -Force
Write-Host $CONFeigenbaumConst

Set-Variable -Name CONFeigenbaumConst -Value 46692016 -Force
Write-Host $CONFeigenbaumConst

Remove-Variable -Name CONFeigenbaumConst
Write-Host $CONFeigenbaumConst

Remove-Variable -Name CONFeigenbaumConst -Force
Write-Host $CONFeigenbaumConst
