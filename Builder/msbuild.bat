@echo off

if "%builder_state%"=="runs" goto :EOF

msbuild %application_path% /p:Configuration=Release;VisualStudioVersion=12.0 2> %msbuild_errors%