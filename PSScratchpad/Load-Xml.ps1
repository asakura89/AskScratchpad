Clear-Host

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)

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
        Throw [System.IO.FileNotFoundException] "$($xmlPath)" 
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

function GetNodeValue($xmlDoc, $selector) {
    $node = $xmlDoc.SelectSingleNode($selector) -As [System.Xml.XmlNode]
    Return $node.InnerText
}

function MapConfigToSmtpSetting($configPath = "$($script:scriptdir)\\smtp.config.xml") {
    $config = LoadXmlFromPath $configPath
    Return @{
        Server = (GetNodeValue $config "configuration/smtpSettings/server")
        Port = (GetNodeValue $config "configuration/smtpSettings/port") -As [System.Int32]
        Username = (GetNodeValue $config "configuration/smtpSettings/userName")
        Password = (GetNodeValue $config "configuration/smtpSettings/password")
        UseTls = (GetNodeValue $config "configuration/smtpSettings/server") -As [System.Boolean]
    }
}

$smtpSettings = MapConfigToSmtpSetting

$smtpSettings | ConvertTo-Json