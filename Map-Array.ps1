Clear-Host

$data = @(
    "Clifford", "Lewis", "Ollie", "Leah", "Kathryn", "Carolyn",
    "Genevieve", "Adam", "Milton", "Eleanor", "Maurice", "Ethel",
    "Charles", "Danny", "Stephen", "Gabriel", "Susan", "Donald",
    "Isabella", "Patrick"
)

$counter = 1
[System.Collections.Generic.List[String]]$mapped = $data |
    Select-Object {("{0}. {1}" -f $script:counter++,$_)} |
    Select-Object -ExpandProperty '("{0}. {1}" -f $script:counter++,$_)'

$mapped

Write-Host "---"

$counter = 1
$mapped = $data |
    Select-Object @{Label="Mapped"; Expression={"{0}. {1}" -f $script:counter++,$_}} |
    Select-Object -ExpandProperty Mapped

$mapped
