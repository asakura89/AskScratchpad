
Clear-Host

function Log([System.String]$message) {
    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logged
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
    Invoke-WebRequest -Method "GET" "https://fake.webaddress.com/"
}
Catch {
    Log "$(GetExceptionMessage $_.Exception)"
}
Finally {
    Log "Get-ExceptionMessage test finish."
}