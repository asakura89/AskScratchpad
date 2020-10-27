Clear-Host

$data = @(
    "Clifford", "Lewis", "Ollie", "Leah", "Kathryn", "Carolyn",
    "Genevieve", "Adam", "Milton", "Eleanor", "Maurice", "Ethel",
    "Charles", "Danny", "Stephen", "Gabriel", "Susan", "Donald",
    "Isabella", "Patrick"
)

$counter = 1
$data |
    Where-Object { $_.ToLower().StartsWith("le") -or $_.ToLower().EndsWith("el") -or ($_[0].ToString().ToLower() -eq "d") } |
    Sort-Object -Descending { $_ } |
    Select-Object @{Label="Sorted"; Expression={"{0}. {1}" -f $Script:counter++,$_}}

