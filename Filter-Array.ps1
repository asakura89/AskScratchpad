Clear-Host

$data = @(
    "Clifford", "Lewis", "Ollie", "Leah", "Kathryn", "Carolyn",
    "Genevieve", "Adam", "Milton", "Eleanor", "Maurice", "Ethel",
    "Charles", "Danny", "Stephen", "Gabriel", "Susan", "Donald",
    "Isabella", "Patrick"
)

$counter = 1
$data |
    Where-Object { $_.ToLower().Contains("an") } |
    Select-Object @{Label="Filtered"; Expression={"{0}. {1}" -f $script:counter++,$_}}

