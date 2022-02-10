@ECHO OFF
@ECHO Checking for .Net and Node versions
@ECHO | set /p=DotNet Version: 
dotnet --version
@ECHO | set /p= Node.js Version: 
node --version
cd scripts
@ECHO starting React App
start cmd /c /wait npmCheck.bat
start appStart.bat
@ECHO starting API server
start dotnetServerStart.bat
exit