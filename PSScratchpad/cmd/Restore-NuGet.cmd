@echo off
goto :init

:init
    echo Restoring NuGet
    set cachednugetdir==%LocalAppData%\NuGet
    set cachednuget=%cachednugetdir%\nuget.latest.exe
    set nugetdwldurl="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
    set nugetdownload=@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%nugetdwldurl%' -OutFile '%cachednuget%'"

:prepare-nuget
    if exist %cachednuget% goto restore-package
    echo Downloading latest version of NuGet.exe ...
    if not exist %LocalAppData%\NuGet md %LocalAppData%\NuGet
    %nugetdownload%

:restore-package
    cd %cachednugetdir%
    %cachednuget% restore "D:\Workspace\Stripped\Stripped.sln"
    %cachednuget% restore "D:\Workspace\Stripped\Stripped-Api.sln"
    goto :exit

:exit
