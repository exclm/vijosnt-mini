@echo off
title Vijos-Mini发布工具
color 0a
if not defined ProgramFiles(x86) set ProgramFiles(x86)=%ProgramFiles%
"%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" /rebuild "Release|x86" "VijosNT Mini.sln"
"%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" /rebuild "Release|x64" "VijosNT Mini.sln"
del pub.rar
rd pub /s /q
md pub
md pub\x86
copy "bin\x86\release\VijosNT Mini.exe" pub\x86
copy "%ProgramFiles(x86)%\SQLite.NET\bin\System.Data.SQLite.dll" pub\x86
md pub\x64
copy "bin\x64\release\VijosNT Mini.exe" pub\x64
copy "%ProgramFiles(x86)%\SQLite.NET\bin\x64\System.Data.SQLite.dll" pub\x64
start /wait "" "%ProgramFiles%\WinRAR\WinRAR.exe" a pub.rar pub
Rem rd pub /s /q
echo 发布完毕
echo.
pause