@ECHO OFF

:loop
IF NOT "%1"=="" (
	IF "%1"=="-version" (
		SET VERSION_NUMBER=%~2
		SHIFT
	)
	IF "%1"=="-apiKey" (
		SET API_KEY=%~2
		SHIFT
	)
	IF "%1"=="-source" (
		SET SOURCE=%~2
		SHIFT
	)
	SHIFT
	GOTO :loop
)

IF "%VERSION_NUMBER%"=="" SET VERSION_NUMBER=1.0.0
IF "%API_KEY%"=="" SET API_KEY=
IF "%SOURCE%"=="" SET SOURCE=

CALL dotnet pack -p:PackageVersion=%VERSION_NUMBER% -p:AssemblyVersion=%VERSION_NUMBER% -p:Version=%VERSION_NUMBER% Keycloak.Migrator/Keycloak.Migrator.csproj -c Release
CALL dotnet nuget push Keycloak.Migrator/nupkg/Keycloak.Migrator.%VERSION_NUMBER%.nupkg --api-key %API_KEY% --source %SOURCE%
