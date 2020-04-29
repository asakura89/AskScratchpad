Clear-Host

$FeigenBaum = 46692

function GetRandomWLowerbound($lowerbound, $upperbound) {
    $seed = [System.Guid]::NewGuid().GetHashCode() % $FeigenBaum
    return $(New-Object System.Random($seed)).Next($lowerbound, $upperbound)
}

function GetRandom($upperbound) {
    return GetRandomWLowerbound 0 $upperbound
}

function GetRandomString($length) {
    $Alphanumeric = $("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z 1 2 3 4 5 6 7 8 9 0 ~ ! @ # $ % ^ & * _ - + = ` | \\ ( ) { } [ ] : ; < > . ? /" -Split " ")
    $randString = @()
    While ($length -Gt 0) {
        $randIdx = GetRandom $($Alphanumeric.Length -1)
        $randChar = $Alphanumeric[$randIdx]
        $randString += $randChar

        $length--
    }

    return [System.String]::Join("", $randString)
}

$rand = GetRandomString 16
Write-Host "Password: $($rand)"