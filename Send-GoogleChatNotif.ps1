param(
    [Parameter(Mandatory=$true)][string]$jsonPath
)

$url = "https://chat.googleapis.com/v1/spaces/AAAACfV4-U8/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=HKStJureHCS2q_9GjqhXRlgqSLIULZ7blrF-Ep9q8QI%3D"
$body = Get-Content -Raw -Path $jsonPath
Write-Host "Sending Message to Google Chat."
$response = Invoke-WebRequest -Method POST -Uri $url -Body $body -ContentType "application/json"
Write-Host "Json Name:" $jsonPath
Write-Host "Status Code:" $response.StatusCode
Write-Host "Content:" $reponse.Content
Write-Host "Done."
