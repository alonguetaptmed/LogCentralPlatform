@echo off
echo Nettoyage et compilation du projet LogCentralPlatform
echo ================================================

echo [1/4] Suppression des dossiers bin et obj...
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
echo Dossiers bin et obj supprimés.

echo [2/4] Nettoyage du cache NuGet...
dotnet nuget locals all --clear
echo Cache NuGet nettoyé.

echo [3/4] Restauration des packages NuGet...
cd %~dp0
dotnet restore --force
if %ERRORLEVEL% NEQ 0 (
    echo Erreur lors de la restauration des packages.
    pause
    exit /b %ERRORLEVEL%
)
echo Restauration terminée avec succès.

echo [4/4] Compilation de la solution...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Erreur lors de la compilation.
    pause
    exit /b %ERRORLEVEL%
)
echo Compilation terminée avec succès.

echo ================================================
echo Opération terminée avec succès.
echo Si vous avez toujours des erreurs, contactez le support technique.
pause
