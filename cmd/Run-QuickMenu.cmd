@echo off
runas /savecred /user:Administrator "C:\Windows\system32\WindowsPowerShell\v1.0\powershell.exe -File \"%~dp0Run-QuickMenu.ps1\""
