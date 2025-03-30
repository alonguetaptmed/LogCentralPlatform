@echo off
echo ====================================================
echo         NETTOYAGE ET RECONSTRUCTION COMPLETE
echo ====================================================
echo.

echo [1/4] Suppression des dossiers bin et obj...
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s/q "%%d"
echo Dossiers bin et obj supprimés.
echo.

echo [2/4] Nettoyage du cache NuGet...
dotnet nuget locals all --clear
echo Cache NuGet nettoyé.
echo.

echo [3/4] Restauration des packages NuGet...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo Erreur lors de la restauration des packages.
    exit /b %ERRORLEVEL%
)
echo Restauration terminée avec succès.
echo.

echo [4/4] Compilation de la solution...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Erreur lors de la compilation.
    exit /b %ERRORLEVEL%
)
echo Compilation terminée avec succès.
echo.

echo ====================================================
echo     OPERATION TERMINEE AVEC SUCCES
echo ====================================================
