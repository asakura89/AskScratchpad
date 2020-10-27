Clear-Host

$temp = @"
<div class='weather-widget'>
    <div class='weather-widget__content'>
        <span class='weather-widget__day'>`${Day}</span>
        <span class='weather-widget__date'>`${Date}</span>
    </div>
    <div class='weather-widget__content'>
        <div class='weather-widget__temp'>
            <i class='icon-weather-sun-cloud'></i>
            <span class='weather-widget__temp__deg'>
                <a href='`${ForecastUrl}' target='_blank'>
                    <span class='weather-widget__temp__deg__max'>`${LowForcastedTemp}</span><span> - `${HighForecastedTemp} &deg;C</span>
                </a>
            </span>
        </div>
        <div class='weather-widget__psi'>
            <span class='weather-widget__psi__info'>24Hr PSI</span>
            <span class='weather-widget__psi__reading'>
                <a href='`${HazeUrl}' target='_blank'>
                    `${LowForecastedPollutantIndex}-`${HighForecastedPollutantIndex}PSI
                </a>
            </span>
        </div>
    </div>
</div>
"@

$rep = @{
    "Day" = [System.DateTime]::Now.DayOfWeek.ToString()
    "Date" = [System.DateTime]::Now.ToString("dd MMMM yyyy")
    "ForecastUrl" = "https://weather-info.fake-api.io"
    "LowForcastedTemp" = "27"
    "HighForecastedTemp" = "36"
    "HazeUrl" = "https://haze-info.fake-api.io"
    "LowForecastedPollutantIndex" = "11.7"
    "HighForecastedPollutantIndex" = "13.0"
}

function Replace([System.String]$template, [System.Collections.Hashtable]$replacements) {
    If ($replacements -Ne $null -And $replacements.Count -Gt 0) {
        $matches = [System.Text.RegularExpressions.Regex]::Matches($template, "\$\{(\w*)\}")
        ForEach ($match In $matches) {
            If ($match.Success) {
                $replace = $match.Groups[0].Value
                $key = $match.Groups[1].Value
                $replacement = $replacements[$key]
                $template = $template.Replace($replace, $replacement)
            }
        }
    }

    Return $template
}


$result = Replace $temp $rep
Write-Host $result
