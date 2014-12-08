@echo off

set PATH=%PATH%;"%USERPROFILE%\AppData\Local\GitHub\PortableGit_ed44d00daa128db527396557813e7b68709ed0e2\bin\";"%WINDIR%\Microsoft.NET\Framework64\v4.0.30319";"%PROGRAMFILES(x86)%\NUnit 2.6.3\bin";"C:\Program Files (x86)\Blat Mail\full"

goto :mail

rmdir /s /q homework

git clone -b master https://github.com/a-baygeldin/homework > git.log

if errorlevel 1 goto :mail

msbuild homework\Builder\Application\Application.sln /p:Configuration=Release;VisualStudioVersion=12.0 2> msbuild_errors.log

if errorlevel 1 goto :mail

nunit-console homework\Builder\Application\Tests\bin\Release\Tests.dll /xml:tests.xml > tests.log

type NUL > exist.log

for /F %%file in libs.txt do (
	if not exist Application\GUI\bin\Release\%%file (
		echo %%file not found >> exist.log
	)
)

:mail
type git.log msbuild_errors.log tests.log exist.log > output.txt
::blat -body output.txt -to lordsofthepigs@gmail.com -from 419880046@mail.ru -server smtp.mail.ru -subject "Builder output" -atf output.log > nul