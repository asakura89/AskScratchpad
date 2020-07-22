

Clear-Host

Get-Process |
    Select-Object `
        @{L="Avg"; E={ [System.Int32]($_.NPM/1024) }}, `
        @{L="Working Set"; E={ [System.Int64]($_.WorkingSet64/1024) }}, `


$

Get-WmiObject Win32_Processor |
    Measure-Object -Property LoadPercentage -Average |
    Select-Object -Last 1 -ExpandProperty Average


$ComputerName = @("localhost")
process {
    foreach ($c in $ComputerName) {
        $avg = Get-WmiObject win32_processor -computername $c | 
                   Measure-Object -property LoadPercentage -Average | 
                   Foreach {$_.Average}
        $mem = Get-WmiObject win32_operatingsystem -ComputerName $c |
                   Foreach {"{0:N2}" -f ((($_.TotalVisibleMemorySize - $_.FreePhysicalMemory)*100)/ $_.TotalVisibleMemorySize)}
        $free = Get-WmiObject Win32_Volume -ComputerName $c -Filter "DriveLetter = 'C:'" |
                    Foreach {"{0:N2}" -f (($_.FreeSpace / $_.Capacity)*100)}
        $script:a = new-object psobject -prop @{ # Work on PowerShell V2 and below
        # [pscustomobject] [ordered] @{ # Only if on PowerShell V3
            ComputerName = $c
            AverageCpu = $avg
            MemoryUsage = $mem
            PercentFree = $free
        }
    }
  }
$a | Format-Table



