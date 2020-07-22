Clear-Host

Get-Process |
    Group-Object -Property ProcessName | 
    Select-Object `
        @{L="Name"; E={ $_.Name }}, `
        @{L="Memory"; E={ "{0:N0}" -F (($_.Group|Measure-Object WorkingSet -Sum).Sum / 1KB) }} |
    Where-Object { $_.Name -Like "*java*" -Or $_.Name -Like "*zookeeper*" -Or $_.Name -Like "*solr*" } |
    Sort-Object { $_.Memory } |
    Format-Table Name, @{N="Mem (KB)"; E={$_.Memory}; A="Right"} -AutoSize

# --------------------------------------------------------------------------------------------------

Clear-Host

Get-Process |
    Group-Object -Property ProcessName | 
    Select-Object `
        @{L="Name"; E={ $_.Name }}, `
        @{L="Mem (KB)"; E={ "{0:N0}" -F (($_.Group|Measure-Object WorkingSet -Sum).Sum / 1KB) }} |
    Sort-Object -Property "Mem (KB)" |
    Out-GridView