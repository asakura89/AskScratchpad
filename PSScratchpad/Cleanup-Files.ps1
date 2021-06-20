Clear-Host

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)
$configPath = [System.IO.Path]::Combine($scriptdir, "notification.config.xml")

function LoadXml([System.String]$xmlContent) {
    $content = $xmlContent.Trim()
    If ([System.String]::IsNullOrEmpty($content)) {
        Return $null
    }

    $xmlDoc = New-Object System.Xml.XmlDocument
    $xmlDoc.LoadXml($content)

    Return $xmlDoc
}

function LoadXmlFromPath([System.String]$xmlPath) {
    If (-Not (Test-Path $xmlPath)) {
        Throw New-Object System.IO.FileNotFoundException($xmlPath)
    }

    $content = [System.IO.File]::ReadAllText($xmlPath)
    [System.Xml.XmlDocument]$xmlDoc = LoadXml $content
    
    Return $xmlDoc
}

function GetAttribute([System.Xml.XmlNode]$node, $name) {
    If ($node -Ne $null -And $node.Attributes -Ne $null) {
        [System.Xml.XmlAttribute]$attr = $node.Attributes[$name]
        If ($attr -Ne $null) {
            Return $attr -As [System.Xml.XmlNode]
        }
    }

    Return $null
}

function GetAttributeValue([System.Xml.XmlNode]$node, $name) {
    $attr = GetAttribute $node $name
    If ($attr -Ne $null) {
        Return $attr.Value
    }

    Return "";
}

function GetNodeValue([System.Xml.XmlDocument]$xmlDoc, $selector) {
    $node = $xmlDoc.SelectSingleNode($selector) -As [System.Xml.XmlNode]
    Return $node.InnerText
}

function GetNodesValue([System.Xml.XmlDocument]$xmlDoc, $selector) {
    $values = New-Object System.Collections.Generic.List[System.String]
    $docs = $xmlDoc.SelectNodes($selector)
    ForEach ($doc In $docs) {
        $values.Add($doc.InnerText)
    }

    Return $values
}

function ListAction($profile) {
    Write-Host "nantokanantoka"
    Write-Host $profile
}

function DeleteAction($profile) {
    Write-Host "nan"
    Write-Host $profile
}

[System.String[]]$quitCommand = @("q", "e", "c")
$inp = Read-Host "Enter to continue or [$quitCommand] to exit"
$quit = [System.Linq.Enumerable]::Contains($quitCommand, $inp.ToLowerInvariant())
While(-Not $quit) {
    $inp = Read-Host "Mode: ([L]ist, [D]elete) "
    $modeActions = @{
        "l" = "ListAction"
        "d" = "DeleteAction"
    }

    If ([System.Linq.Enumerable]::Contains([System.String[]]$modeActions.Keys, $inp.ToLowerInvariant())) {
        & $modeActions[$inp.ToLowerInvariant()] "profilename"
    }

    Write-Host "Done."

    $inp = Read-Host "Enter to continue or [$quitCommand] to exit"
    $quit = [System.Linq.Enumerable]::Contains($quitCommand, $inp.ToLowerInvariant())
}
