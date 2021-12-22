Clear-Host

function GetScriptInfo() {
    $scriptfile = (Get-Item $PSCommandPath)

    Return @{
        Name = $scriptfile.BaseName
        FullName = $scriptfile.Name
        Directory = $scriptfile.Directory
    }
}

function Log($message, $starting = $false, $writeToScreen = $true) {
    $scriptInfo = GetScriptInfo
    $logname = "$($scriptInfo.Name)_$([System.DateTime]::Now.ToString("yyyyMMddHHmm")).log"
    $logfile = [System.IO.Path]::Combine($scriptInfo.Directory, $logname)

    $logmsg = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    If ($writeToScreen) {
        Write-Host $logmsg
    }

    If ($starting) {
        $logmsg | Out-File -Encoding "UTF8" -FilePath $logfile
    }
    Else {
        $logmsg | Add-Content -Encoding "UTF8" -Path $logfile
    }
}

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

function GetConfig() {
    $scriptInfo = GetScriptInfo
    $configFileName = [System.IO.Path]::Combine($scriptInfo.Directory, "$($scriptInfo.Name).config.xml")

    Return LoadXmlFromPath $configFileName
}

Log ":: Start Script ::" $true

Log "Doing something ..."

Log "Doing another thing ..."

Log ":: Finish Script ::"
