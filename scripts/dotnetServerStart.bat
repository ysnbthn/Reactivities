@ECHO OFF
cd ..
cd API
@ECHO Starting API
dotnet restore
dotnet watch run 
