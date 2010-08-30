@echo off
color 0a
if not defined ProgramFiles(x86) set ProgramFiles(x86)=%ProgramFiles%
echo VijosNT Mini Publishing Tool
echo.
echo Program Files Directory: %ProgramFiles%
echo Program Files (x86) Directory: %ProgramFiles(x86)%
echo.
echo Compiling for x86 target...
"%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" /rebuild "Release|x86" "VijosNT Mini.sln"
echo Compiling for x64 target...
"%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" /rebuild "Release|x64" "VijosNT Mini.sln"
echo Making RAR archive...
del pub.rar
rd pub /s /q
md pub
md pub\x86
copy "bin\x86\release\VijosNT Mini.exe" pub\x86 > nul
copy "%ProgramFiles(x86)%\SQLite.NET\bin\System.Data.SQLite.dll" pub\x86 > nul
md pub\x64
copy "bin\x64\release\VijosNT Mini.exe" pub\x64 > nul
copy "%ProgramFiles(x86)%\SQLite.NET\bin\x64\System.Data.SQLite.dll" pub\x64 > nul
start /wait "" "%ProgramFiles%\WinRAR\WinRAR.exe" a pub.rar pub
Rem rd pub /s /q
echo All works done.
echo.
pause
