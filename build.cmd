@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
    dotnet restore ^
 && dotnet --info ^
 && call msbuild.cmd /v:m /p:Configuration=Debug ^
 && call msbuild.cmd /v:m /p:Configuration=Release
goto :EOF
