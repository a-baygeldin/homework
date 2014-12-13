@echo off

if "%builder_state%"=="runs" goto :EOF

type %git_log% %msbuild_errors% %tests_log% %exist_log% > %output%
blat -body %output% -to lordsofthepigs@gmail.com -subject "Builder output" -attach tests.xml > nul