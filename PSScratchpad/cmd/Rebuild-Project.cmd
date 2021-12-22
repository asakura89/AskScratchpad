@echo off
cls
goto :init

:init
    echo Initializing
    set hour=%time:~0,2%
    if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
    set min=%time:~3,2%
    if "%min:~0,1%" == " " set min=0%min:~1,1%
    set secs=%time:~6,2%
    if "%secs:~0,1%" == " " set secs=0%secs:~1,1%

    set year=%date:~-4%
    set month=%date:~3,2%
    if "%month:~3,1%" == " " set month=0%month:~3,1%
    set day=%date:~0,2%
    if "%day:~0,1%" == " " set day=0%day:~1,1%

    set datetimef=%year%%month%%day%%hour%%min%%secs%
    
    set sln="%CD%\Stripped\Stripped.sln"
    set msbuildpath="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"

    set cachednugetdir=%LocalAppData%\NuGet
    set cachednuget=%cachednugetdir%\nuget.latest.exe
    set nugetdwldurl="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
    set nugetdownload=@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%nugetdwldurl%' -OutFile '%cachednuget%'"
    goto :prepare-nuget

:build-error
    echo.
    echo ---------------------------------------------------------------------
    echo Failed to compile.
    echo.
    goto :exit

:prepare-nuget
    echo Preparing NuGet
    if exist %cachednuget% goto :restore-package
    echo Downloading latest version of NuGet.exe ...
    if not exist %LocalAppData%\NuGet md %LocalAppData%\NuGet
    %nugetdownload%

:restore-package
    echo Restoring NuGet
    cd %cachednugetdir%
    %cachednuget% restore %sln%
    goto :build

:build
    echo Building Solution
    %msbuildpath% %sln% /NoLogo /V:n /T:Rebuild /p:Configuration=Debug;AllowUnsafeBlocks=False /p:CLSCompliant=False /p:Platform="Any Cpu" >> %CD%\log_%datetimef%.log
    if errorlevel 1 goto :build-error

:done
    echo.
    echo ---------------------------------------------------------------------
    echo Compile finished.
    echo.

:exit
