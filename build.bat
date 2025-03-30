@echo off
echo ====================================================
echo     RESTAURATION ET COMPILATION DE LA SOLUTION
echo ====================================================
echo.

echo [1/2] Restauration des packages NuGet...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo Erreur lors de la restauration des packages.
    exit /b %ERRORLEVEL%
)
echo Restauration terminée avec succès.
echo.

echo [2/2] Compilation de la solution...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Erreur lors de la compilation.
    exit /b %ERRORLEVEL%
)
echo Compilation terminée avec succès.
echo.

echo ====================================================
echo     OPÉRATION TERMINÉE AVEC SUCCÈS
echo ====================================================
