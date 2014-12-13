@echo off

if "%builder_state%"=="runs" goto :EOF

git clone -b master %repository% > %git_log%