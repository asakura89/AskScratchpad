Clear-Host

Import-Module WebAdministration

function ExtractUrl([System.String]$binding) {
    $splittedBinding = $binding -Split ":"
    $url = $splittedBinding[0] + "://" + $splittedBinding[3] + ":" + $splittedBinding[2]

    Return $url
}

function GetIISSiteInfo() {
    $siteInfos =
        Get-ChildItem IIS:\Sites |
            Select-Object @{L="Urls";E={ `
                ($_.Bindings.Collection | `
                    Select-Object @{L="InnerItem";E={ (ExtractUrl "$($_.protocol):$($_.bindingInformation)") }} | `
                    Select-Object -ExpandProperty InnerItem
                ) `
            }},
            @{L="Name";E={ $_.Name.ToString() }},
            @{L="State";E={ $_.State.ToString() }}

    Return $siteInfos
}

GetIISSiteInfo |
    Format-Table