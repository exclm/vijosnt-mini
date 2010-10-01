@echo off
if not defined ProgramFiles(x86) set ProgramFiles(x86)=%ProgramFiles%
echo VijosNT Mini Publishing Tool
echo.
echo Program Files Directory: %ProgramFiles%
echo Program Files (x86) Directory: %ProgramFiles(x86)%
echo.
echo Compiling...
"%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" /rebuild "Release|Any CPU" "VijosNT Mini.sln"
echo Making RAR archive...
del pub.rar
rd pub /s /q
md pub
md pub\x86
copy "bin\Any CPU\release\VijosNT Mini.exe" pub\x86 > nul
copy "%ProgramFiles(x86)%\SQLite.NET\bin\System.Data.SQLite.dll" pub\x86 > nul
md pub\x64
copy "bin\Any CPU\release\VijosNT Mini.exe" pub\x64 > nul
copy "%ProgramFiles(x86)%\SQLite.NET\bin\x64\System.Data.SQLite.dll" pub\x64 > nul
cd pub
"%ProgramFiles%\WinRAR\Rar.exe" a -r pub.rar
cd..
copy pub\pub.rar > nul
rd pub /s /q
echo All works done.
echo.
pause
