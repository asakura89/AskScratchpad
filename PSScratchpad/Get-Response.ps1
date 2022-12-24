
Clear-Host

function Log([System.String]$message) {
    $logmsg = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logmsg
}

function GetExceptionMessage([System.Exception]$ex) {
    $errorList = New-Object System.Text.StringBuilder
    [System.Exception]$current = $ex;
    While ($current -Ne $null) {
        [void]$errorList.AppendLine()
        [void]$errorList.AppendLine("Exception: $($current.GetType().FullName)")
        [void]$errorList.AppendLine("Message: $($current.Message)")
        [void]$errorList.AppendLine("Source: $($current.Source)")
        [void]$errorList.AppendLine($current.StackTrace)
        [void]$errorList.AppendLine()

        $current = $current.InnerException
    }

    Return $errorList.ToString()
}

Try {
    Log "Get-ExceptionMessage test start."
    Invoke-WebRequest -Method "GET" "https://graph.microsoft.com/v1.0/users"
}
Catch {
    Log (GetExceptionMessage $_.Exception)
    $res = [System.Net.HttpWebResponse]$_.Exception.Response
    Log (ConvertTo-Json $res)
}
Finally {
    Log "Get-ExceptionMessage test finish."
}