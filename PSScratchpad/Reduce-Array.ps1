Clear-Host

$data = @(
    "Clifford", "Lewis", "Ollie", "Leah", "Kathryn", "Carolyn",
    "Genevieve", "Adam", "Milton", "Eleanor", "Maurice", "Ethel",
    "Charles", "Danny", "Stephen", "Gabriel", "Susan", "Donald",
    "Isabella", "Patrick"
)

$counter = 1
$mapped = $data |
    Sort-Object -Descending { $_ } |
    Select-Object `
        @{ Name = "Index";Expression = {($Script:counter++)} }, `
        @{ Name = "Name";Expression = {$_} }

$even = $mapped |
    Where-Object { $_.Index % 2 -eq 0 } |
    ForEach-Object `
        -Begin { $start = "" } `
        -Process { $start = $start + $_.Index + ": " + $_.Name + ", " } `
        -End { $start.Substring(0, $start.Length -2) }

$odd = $mapped |
    Where-Object { $_.Index % 2 -ne 0 } |
    ForEach-Object `
        -Begin { $start = "" } `
        -Process { $start = $start + $_.Index + ": " + $_.Name + ", " } `
        -End { $start.Substring(0, $start.Length -2) }

$even
$odd