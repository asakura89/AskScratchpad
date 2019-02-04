@echo off
goto :init

:init
    set cakescript=%1

    set cachednuget=%LocalAppData%\NuGet\nuget.latest.exe
    set toolsnuget=tools\nuget.exe

    set cakepackageconfig=tools\packages.config
    set toolscake=tools\Cake\Cake.exe

    set nugetdwldurl="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
    set cakepkgdwldurl="https://cakebuild.net/download/bootstrapper/packages"

    set nugetdownload=@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%nugetdwldurl%' -OutFile '%cachednuget%'"
    set cakedownload=@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%cakepkgdwldurl%' -OutFile '%cakepackageconfig%'"

:prepare-tools
    if exist tools goto :prepare-nuget
    echo Creating tools directory ...
    md tools

:prepare-nuget
    if exist %cachednuget% goto copy-nuget
    echo Downloading latest version of NuGet.exe ...
    if not exist %LocalAppData%\NuGet md %LocalAppData%\NuGet
    %nugetdownload%

:copy-nuget
    if exist %toolsnuget% goto prepare-cake
    echo Copying NuGet.exe ...
    copy %cachednuget% %toolsnuget% > nul

:prepare-cake
    if exist %cakepackageconfig% goto restore-cake
    echo Download Cake package.config ...
    %cakedownload%

:restore-cake
    if exist %toolscake% goto call-cake
    cd %CD%\tools
    nuget.exe install -ExcludeVersion -OutputDirectory .
    cd ..

:call-cake
    rem %toolscake% %cakescript% --verbosity=Verbose > build-log.txt
    %toolscake% %cakescript%
    goto :exit

:exit