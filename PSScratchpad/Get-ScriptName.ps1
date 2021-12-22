Clear-Host

@{
    PSCommandPath = $PSCommandPath
    SplitPathResolve = $(Split-Path -Resolve $PSCommandPath)
    SplitPathParent = $(Split-Path -Parent $PSCommandPath)
    SplitPathParentResolve = $(Split-Path -Parent -Resolve $PSCommandPath)
    SplitPathLeaf = $(Split-Path -Leaf $PSCommandPath)
    SplitPathLeafResolve = $(Split-Path -Leaf -Resolve $PSCommandPath)
    DotNetFileBaseName = (Get-Item $PSCommandPath).BaseName
} | 
Format-List

function GetScriptInfo() {
    $scriptfile = (Get-Item $PSCommandPath)

    Return @{
        Name = $scriptfile.BaseName
        FullName = $scriptfile.Name
        Directory = $scriptfile.Directory
    }
}

GetScriptInfo
