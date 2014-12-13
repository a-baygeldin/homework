@echo off

set builder_state="runs"

call settings.bat

call clean.bat

call checkout.bat

if errorlevel 1 goto :mail

call msbuild.bat

if errorlevel 1 goto :mail

call nunit.bat

type nul > %exist_log%

for /F %%file in %libs% do (
	if not exist %release_path%%%file (
		echo %%file not found >> %exist_log%
	)
)

if 1 == 1 goto :mail

:mail

call report.bat 