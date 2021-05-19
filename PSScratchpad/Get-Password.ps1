Clear-Host

$FeigenBaum = 46692016

function GetRandomWLowerbound([System.Int32]$lowerbound, [System.Int32]$upperbound) {
    $seed = [System.Guid]::NewGuid().GetHashCode() % $FeigenBaum
    return $(New-Object System.Random($seed)).Next($lowerbound, $upperbound)
}

function GetRandom([System.Int32]$upperbound) {
    return GetRandomWLowerbound 0 $upperbound
}

function GetRandomString([System.Int32]$length) {
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

$Base64Plus = "+";
$Base64Slash = "/";
$Base64Underscore = "_";
$Base64Minus = "-";
$Base64Equal = "=";
$Base64DoubleEqual = "==";
[System.Char]$Base64EqualChar = "=";

function EncodeBase64Url([System.String]$plain) {
    return EncodeBase64UrlFromBytes(
        [System.Text.Encoding]::UTF8.GetBytes($plain)
    )
}

function EncodeBase64UrlFromBytes([System.Byte[]]$bytes) {
    return [System.Convert]::
        ToBase64String($bytes).
        TrimEnd($Base64EqualChar).
        Replace($Base64Plus, $Base64Minus).
        Replace($Base64Slash, $Base64Underscore)
}

function GenerateKey() {
    return EncodeBase64Url $(GetRandomString 64) 
}

function GenerateSalt() {
    return EncodeBase64Url $(GetRandomString 128)
}

function GetAlgorithm([System.String]$securityKey, [System.String]$securitySalt) {
    $saltBytes = [System.Text.Encoding]::UTF8.GetBytes($securityKey + $securitySalt)
    $randByte = New-Object System.Security.Cryptography.Rfc2898DeriveBytes($securityKey, $saltBytes, 12000)

    # 256 Not supported on Powershell Core 6
    # https://github.com/dotnet/runtime/issues/895
    $MaxOutSize = 128 
    [System.Int32]$MaxOutSizeInBytes = $MaxOutSize / 8
    $algorithm = New-Object System.Security.Cryptography.RijndaelManaged
    $algorithm.BlockSize = $MaxOutSize
    $algorithm.Key = $randByte.GetBytes($MaxOutSizeInBytes)
    $algorithm.IV = $randByte.GetBytes($MaxOutSizeInBytes)
    $algorithm.Mode = [System.Security.Cryptography.CipherMode]::CBC
    $algorithm.Padding = [System.Security.Cryptography.PaddingMode]::PKCS7

    return $algorithm
}

function Encrypt([System.String]$plainText, [System.String]$securityKey, [System.String]$securitySalt) {
    $algorithm = $null
    try {
        $algorithm = GetAlgorithm $securityKey $securitySalt
        $plainBytes = [System.Text.Encoding]::UTF8.GetBytes($plainText);
        $cipherBytes = $null;
        $stream = $null

        try {
            $stream = New-Object System.IO.MemoryStream
            $cryptoStream = $null

            try {
                $cryptoStream = New-Object System.Security.Cryptography.CryptoStream(
                    $stream, 
                    $algorithm.CreateEncryptor(), 
                    [System.Security.Cryptography.CryptoStreamMode]::Write
                )
                $cryptoStream.Write($plainBytes, 0, $plainBytes.Length);
            }
            finally {
                if ($cryptoStream -Ne $null -And $cryptoStream -Is [System.IDisposable]) {
                    $cryptoStream.Dispose()
                }
            }

            $cipherBytes = $stream.ToArray();
        }
        finally {
            if ($stream -Ne $null -And $stream -Is [System.IDisposable]) {
                $stream.Dispose()
            }
        }

        return EncodeBase64UrlFromBytes $cipherBytes
    }
    finally {
        if ($algorithm -Ne $null -And $algorithm -Is [System.IDisposable]) {
            $algorithm.Dispose()
        }
    }
}

$key = GenerateKey
Write-Host "Key: $($key)"

$salt = GenerateSalt
Write-Host "Salt: $($salt)"

# because the password encryption is one-way
# there's no reason to show $key and $salt
# and also no reason to use same $key and $salt everytime
$encrypted = Encrypt "Hello world" $key $salt
Write-Host "Encrypted: $($encrypted)"
