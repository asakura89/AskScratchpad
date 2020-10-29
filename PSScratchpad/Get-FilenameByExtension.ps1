Clear-Host

function GetFiles($rootDir, $fileExt) {
    If (-Not $fileExt.StartsWith(".")) {
        $fileExt = ".$($fileExt)"
    }

    $fileExt = "*$($fileExt)"

    $files = Get-ChildItem -Path $rootDir -Include $fileExt -Recurse |
        Select-Object -ExpandProperty VersionInfo  |
        Select-Object -ExpandProperty FileName

    Return $files
}

$dir = "C:\Users\<Username>\.nuget\packages"
$ext = ".nupkg"

GetFiles $dir $ext |
    Sort-Object -Descending
