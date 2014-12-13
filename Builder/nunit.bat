@echo off

if "%builder_state%"=="runs" goto :EOF

nunit-console %tests_path% /xml:tests.xml > %tests_log%