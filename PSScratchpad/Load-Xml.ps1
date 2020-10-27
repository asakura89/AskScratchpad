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

function MapConfigToSmtpSetting($configPath = $Script:configPath) {
    $config = LoadXmlFromPath $configPath
    Return @{
        Server = (GetNodeValue $config "configuration/smtpSettings/server")
        Port = (GetNodeValue $config "configuration/smtpSettings/port") -As [System.Int32]
        Username = (GetNodeValue $config "configuration/smtpSettings/userName")
        Password = (GetNodeValue $config "configuration/smtpSettings/password")
        UseTls = (GetNodeValue $config "configuration/smtpSettings/useTls") -As [System.Boolean]
    }
}

$smtpSettings = MapConfigToSmtpSetting
$smtpSettings | ConvertTo-Json

$xmlContent = @"
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <events>
        <event>name.of.the.event</event>
        <event>name.of.the.second.event</event>
    </events>
</configuration>
"@

$xml = LoadXml $xmlContent
$values = (GetNodesValue $xml "configuration/events/event")
$values | ConvertTo-Json
