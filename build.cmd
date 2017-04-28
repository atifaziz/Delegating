@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
    dotnet restore ^
 && dotnet --info ^
 && dotnet build /p:Configuration=Debug ^
 && dotnet build /p:Configuration=Release
goto :EOF
