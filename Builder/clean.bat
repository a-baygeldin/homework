@echo off

if "%builder_state%"=="runs" goto :EOF

rmdir /s /q %repository_name%