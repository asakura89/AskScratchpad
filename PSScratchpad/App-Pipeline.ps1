Clear-Host

#:< Log >:#
function Log($message, $starting = $false, $writeToScreen = $true) {
    $scriptfile = (Get-Item $PSCommandPath)
    $logdir = $scriptfile.Directory
    $logname = "$($scriptfile.BaseName)_$([System.DateTime]::Now.ToString("yyyyMMddHHmm")).log"
    $logfile = [System.IO.Path]::Combine($logdir, $logname)

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
#:< Log >:#

#:< GetException >:#
function GetExceptionMessage([System.Exception]$ex) {
    $errorList = New-Object System.Text.StringBuilder
    [System.Exception]$current = $ex;
    While ($current -Ne $null) {
        [void]$errorList.AppendLine()
        [void]$errorList.AppendLine("Exception: $($current.GetType().FullName)")
        [void]$errorList.AppendLine("Message: $($current.Message)")
        [void]$errorList.AppendLine("Source: $($current.Source)")
        [void]$errorList.AppendLine($current.StackTrace)
        [void]$errorList.AppendLine()

        $current = $current.InnerException
    }

    Return $errorList.ToString()
}
#:< GetException >:#

#:< Load Xml >:#
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

function AssignAttributeTo([System.Xml.XmlDocument]$xmlDoc, [System.Xml.XmlNode]$node, $name, $value) {
    If ($xmlDoc -Ne $null -And $node -Ne $null) {
        $attr = GetAttribute $node $name
        If ($attr -Eq $null) {
            $attr = $xmlDoc.CreateAttribute($name)
            $node.Attributes.Append($attr)
        }

        $attr.Value = $value
    }
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

function GetMultipleNodeValue([System.Xml.XmlDocument]$xmlDoc, $selector) {
    $values = New-Object System.Collections.Generic.List[System.String]
    $docs = $xmlDoc.SelectNodes($selector)
    ForEach ($doc In $docs) {
        $values.Add($doc.InnerText)
    }

    Return $values
}
#:< Load Xml >:#

#:< Reflection >:#
$TypeAndAssemblyRegex = "^(?<TypeN>.+)(?:,\\s{1,}?)(?<AsmN>.+)$"
$rgxOptions = [System.Text.RegularExpressions.RegexOptions]::Compiled `
    -Bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase `
    -Bor [System.Text.RegularExpressions.RegexOptions]::Singleline
$typeNameRgx = New-Object System.Text.RegularExpressions.Regex($TypeAndAssemblyRegex, $rgxOptions)

function Parse ([System.String]$source) {
    $typeName = $typeNameRgx.Match($source).Groups["TypeN"].Value
    $asmName = $typeNameRgx.Match($source).Groups["AsmN"].Value
    If ([System.String]::IsNullOrEmpty($typeName) -Or [System.String]::IsNullOrEmpty($asmName)) {
        Throw System.InvalidOperationException "Wrong Type or Assembly. '$($source)'."
    }

    Return @{
        Type = $typeName
        Assembly = $asmName
    }
}

function GetType($typeAndAsm) {
    If ($typeAndAsm -Eq $null) {
        Throw System.ArgumentNullException "typeAndAsm"
    }

    [System.Reflection.Assembly]$asm = $null
    $appDAsms = [System.AppDomain]::CurrentDomain.GetAssemblies()
    ForEach ($appDAsm In $appDAsms) {
        If ($appDAsm.GetName().Name -Eq $typeAndAsm.Assembly) {
            $asm = $appDAsm
            Break
        }
    }

    If ($asm -Eq $null) {
        Throw System.InvalidOperationException "Assembly '$($typeAndAsm.Assembly)' was not found."
    }

    Return GetType $typeAndAsm $asm
}

function GetType ($typeAndAsm, [System.Reflection.Assembly]$asm) {
    If ($typeAndAsm -Eq $null) {
        Throw System.ArgumentNullException $typeAndAsm
    }

    If ($asm -Eq $null) {
        Throw System.ArgumentNullException $asm
    }

    [System.Type]$type = $null
    $asmTypes = $asm.GetTypes()
    ForEach ($asmType In $asmTypes) {
        If ($asmType.FullName.Replace("+", ".") -Eq $typeAndAsm.Type) {
            $type = $asmType
            Break
        }
    }

    If ($type -Eq $null) {
        Throw System.InvalidOperationException "Type '$($typeAndAsm.Type)' was not found. Assembly '$($typeAndAsm.Assembly)'."
    }

    Return $type
}
#:< Reflection >:#

#:< Pipeline >:#
function MapConfigToActionDefinition([System.Xml.XmlNode]$actionConfig) {
    $typeValue = $actionConfig.GetAttributeValue("type");
    $methodValue = $actionConfig.GetAttributeValue("method");
    $actionTypeNAsm = Parse $typeValue

    If ([System.String]::IsNullOrEmpty($methodValue)) {
        Throw System.InvalidOperationException "Wrong Method configuration. '$($methodValue)'."
    }

    Return @{
        Type = $actionTypeNAsm.Type
        Assembly = $actionTypeNAsm.Assembly
        Method = $methodValue
    }
}

function MapConfigToPipelineDefinition([System.Xml.XmlNode]$pipelineConfig) {
    $actionsConfig = [System.Linq.Enumerable].
        GetMethod("Cast").
        MakeGenericMethod([System.Xml.XmlNode].GetType()).
        Invoke($pipelineConfig.SelectNodes("action"))

    $actions = @()
    ForEach ($actionConfig In $actionsConfig) {
        $action = MapConfigToActionDefinition $actionConfig
        $actions.Add($action)
    }

    $nameValue = GetAttributeValue $pipelineConfig "name"
    $ctxTypeValue = GetAttributeValue $pipelineConfig "contextType"
    If ([System.String]::IsNullOrEmpty($nameValue)) {
        Throw System.InvalidOperationException "Pipeline Name was not defined."
    }

    If ([System.String]::IsNullOrEmpty($ctxTypeValue)) {
        Throw System.InvalidOperationException "Pipeline ContextType was not defined."
    }

    $ctxTypeNAsm = Parse $ctxTypeValue

    Return @{
        Name = $nameValue
        ContextType = $ctxTypeNAsm.Type
        ContextAssembly = $ctxTypeNAsm.Assembly
        Actions = $actions
    }
}

$pipelineDefinitions = @()
function LoadPipeline($configPath) {
    If ([System.String]::IsNullOrEmpty($configPath)) {
        $scriptdir = (Get-Item $PSCommandPath).Directory
        $configPath = [System.IO.Path]::Combine($scriptdir, "pipeline.config.xml")
    }

    $config = LoadFromPath $configPath
    $pipelinesSelector = "configuration/pipelines";
    $pipelinesConfig = [System.Linq.Enumerable].
        GetMethod("Cast").
        MakeGenericMethod([System.Xml.XmlNode].GetType()).
        Invoke($config.SelectNodes($pipelinesSelector))

    If ($pipelinesConfig -Eq $null -Or -Not([System.Linq.Enumerable]::Any((,$pipelinesConfig)))) {
        Throw System.InvalidOperationException "$($pipelinesSelector) wrong configuration."
    }

    $pipelines = @()
    ForEach ($pipelineConfig In $pipelinesConfig) {
        $pipeline = MapConfigToPipelineDefinition $pipelineConfig
        $pipelines.Add($pipeline)
    }

    $pipelineDefinitions = $pipelines
}

function Execute($pipelineName, $context) {
    $pipeline = $null
    ForEach ($definition In $pipelineDefinitions) {
        If ($definition.Name.Equals($pipelineName, [System.StringComparison]::OrdinalIgnoreCase)) {
            $pipeline = $definition
            Break
        }
    }

    ForEach ($action in $pipeline.Actions) {
        $typeNAsm = @{
            Type = $action.Type
            Assembly = $action.Assembly
        }

        $type = GetType $typeNAsm
        $instance = [System.Activator]::CreateInstance($type);
        $bindingOptions = [System.Reflection.BindingFlags]::Instance -Bor [System.Reflection.BindingFlags]::Public
        $methods = $instance.
            GetType().
            GetMethods($bindingOptions)

        $methodInfo = $null
        ForEach ($method In $methods) {
            If ($method.Name -Eq $action.Method) {
                $methodInfo = $method
                Break
            }
        }

        If ($methodInfo -Eq $null) {
            Throw System.InvalidOperationException "Method '$($action.Method)' was not found. Type '$($action.Type)', Assembly '$($action.Assembly)'."
        }
                
        $methodInfo.Invoke($instance, @(,$context));
        If ($context.Cancelled) {
            Break
        }
    }

    Return $context.ActionMessages
}
#:< Pipeline >:#

#:< ------------------------------------------------------------------------------ >:#

#:< Helper >:#

function HandleDefaultPipeline($pipelineName) {
    $pipelineContext = @{
        ActionMessages = @()
        Context = $null
        Cancelled = $false
    }

    $result = Execute $pipelineName $pipelineContext
    If ($pipelineContext.Cancelled) {
        Log "Pipeline is cancelled."
    }

    If ([System.Linq.Enumerable]::Any($result)) {
        $infos = $result |
            Where-Object { $_.ResponseType -Eq "I" } |
            Select-Object -ExpandProperty Message

        ForEach ($info In $infos) {
            Log $info
        }

        $warns = $result |
            Where-Object { $_.ResponseType -Eq "W" } |
            Select-Object -ExpandProperty Message

        ForEach ($warn In $warns) {
            Log $warn
        }

        $successes = $result |
            Where-Object { $_.ResponseType -Eq "S" } |
            Select-Object -ExpandProperty Message

        ForEach ($success In $successes) {
            Log $success
        }

        $errors = $result |
            Where-Object { $_.ResponseType -Eq "E" } |
            Select-Object -ExpandProperty Message

        ForEach ($error In $errors) {
            Log $error
        }

        If ([System.Linq.Enumerable]::Any($errors)) {
            Return $null
        }
    }

    Return $pipelineContext.Context
}

#:< Helper >:#

Log "``:: Start Script ::'" $true

LoadPipeline [System.IO.Path]::Combine($scriptdir, "pipeline.config.xml")
HandleDefaultPipeline "pipe:ApplicationStart"

Log ".:: Finish Script ::."
