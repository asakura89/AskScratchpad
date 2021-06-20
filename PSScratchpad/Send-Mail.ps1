Clear-Host

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)

function Log($message) {
    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logged
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

function MapConfigToSmtpSetting($configPath = "$($Script:scriptdir)\\smtp.config.xml") {
    $config = LoadXmlFromPath $configPath
    Return @{
        Server = (GetNodeValue $config "configuration/smtpSettings/server")
        Port = (GetNodeValue $config "configuration/smtpSettings/port") -As [System.Int32]
        Username = (GetNodeValue $config "configuration/smtpSettings/userName")
        Password = (GetNodeValue $config "configuration/smtpSettings/password")
        UseTls = (GetNodeValue $config "configuration/smtpSettings/server") -As [System.Boolean]
    }
}

function SendMail ($smtpConfig, $mailConfig) {
    If ([System.String]::IsNullOrEmpty($mailConfig.From)) {
        Log "Mail Sender not found!"
        Return
    }

    If (-Not ([System.Linq.Enumerable]::Any($mailConfig.Receiver))) {
        Log "Mail Receiver not found!"
        Return
    }

    $client = New-Object System.Net.Mail.SmtpClient
    $message = New-Object System.Net.Mail.MailMessage

    ForEach ($address In $mailConfig.Receiver) {
        $message.To.Add($address)
    }

    If (-Not ([System.String]::IsNullOrEmpty($mailConfig.Cc))) {
        $message.CC.Add($mailConfig.Cc)
    }

    If (-Not ([System.String]::IsNullOrEmpty($mailConfig.From))) {
        $message.From = New-Object System.Mail.MailAddress($mailConfig.From, $mailConfig.FromName)
    }
    Else {
        $message.From = New-Object System.Mail.MailAddress($mailConfig.From)
    }

    $message.Subject = $mailConfig.Subject
    $message.Body = $mailConfig.Body
    $message.IsBodyHtml = $True

    Log "Send email."
    Log $message

    Try {
        $client.Send($message)
    }
    Catch {
        Log "Unable to send email."
        Throw
    }

    Log "Mail sent."
}

Try {
    $smtpConfig = MapConfigToSmtpSetting
    

    SendMail $smtpConfig
}
Catch {
    Log $("Error caught: `n" + `
        $_.Exception.ItemName + " `n" + `
        $_.Exception.Message + " `n" + `
        $_.Exception.StackTrace)
}