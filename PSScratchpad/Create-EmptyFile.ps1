Clear-Host

$files = "index.js","index.html","style.css"
New-Item -ItemType File $files
$(Get-ChildItem $files).ForEach({ $_.LastWriteTime = Get-Date })

# ^ will output 0 kb file

Write-Output $null > anotherfile.txt

# ^ will output 1 kb file

# Silencing output
New-Item -ItemType File $files | Out-Null
$null = New-Item -ItemType File $files
[void](New-Item -ItemType File $files)

