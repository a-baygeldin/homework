@echo off

if "%builder_state%"=="runs" goto :EOF

set git_path="%USERPROFILE%\AppData\Local\GitHub\PortableGit_ed44d00daa128db527396557813e7b68709ed0e2\bin"
set msbuild_path="%WINDIR%\Microsoft.NET\Framework64\v4.0.30319"
set nunit_path="%PROGRAMFILES(x86)%\NUnit 2.6.3\bin"
set blat_path="%PROGRAMFILES(x86)%\Blat Mail\full"

set PATH=%PATH%;%git_path%;%msbuild_path%;%nunit_path%;%blat_path%

set repository="https://github.com/a-baygeldin/homework"
set repository_name="homework"
set application_path="homework\Builder\Application\Application.sln"
set tests_path="homework\Builder\Application\Tests\bin\Release\Tests.dll"
set release_path="homework\Builder\Application\GUI\bin\Release\"
set libs=libs.txt

set git_log=git.log
set msbuild_errors=msbuild_errors.log
set tests_log=tests.log
set exist.log=exist.log

set output=output.txt
