@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
set VERSION_SUFFIX=
if not "%~1"=="" set VERSION_SUFFIX=/p:VersionSuffix=%1
call build ^
  && call msbuild.cmd /v:m /t:Pack                        ^
                           /p:Configuration=Release       ^
                           /p:IncludeSymbols=true         ^
                           /p:IncludeSource=true          ^
                           /p:PackageOutputPath=%cd%/dist ^
                           %VERSION_SUFFIX%               ^
                           src\Delegating.csproj
goto :EOF
