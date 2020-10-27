Clear-Host

function Log($message) {
    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logged
}

Try {
    Try {
        Throw New-Object System.InvalidOperationException("yaharo")
    }
    Catch {
        Log $("Exception thrown: `n" + `
            $_.Exception.ItemName + " `n" + `
            $_.Exception.Message + " `n" + `
            $_.Exception.StackTrace)

        Throw
    }

    Try {
        Write-Error -Message "yaharo" -ErrorAction Stop
    }
    Catch {
        Log $("Error written: `n" + `
            $_.Exception.ItemName + " `n" + `
            $_.Exception.Message + " `n" + `
            $_.Exception.StackTrace)

        Throw
    }
}
Catch {
    Log $("Error caught: `n" + `
        $_.Exception.ItemName + " `n" + `
        $_.Exception.Message + " `n" + `
        $_.Exception.StackTrace)
}