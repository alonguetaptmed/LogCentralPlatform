@echo off
echo *****************************************************
echo * SCRIPT DE NETTOYAGE COMPLET LOGCENTRALPLATFORM    *
echo *****************************************************
echo.

echo 1. Suppression de TOUS les dossiers bin et obj...
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
echo FAIT.
echo.

echo 2. Nettoyage du cache NuGet...
dotnet nuget locals all --clear
echo FAIT.
echo.

echo 3. Restauration FORCEE des packages NuGet...
dotnet restore --force
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR lors de la restauration des packages.
    echo Appuyez sur une touche pour quitter.
    pause > nul
    exit /b %ERRORLEVEL%
)
echo FAIT.
echo.

echo 4. Exécution du Build...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR lors du build. C'est une erreur de code, pas une erreur de compilation.
    echo Regardez les erreurs ci-dessus pour identifier le problème.
    echo.
    echo Appuyez sur une touche pour quitter.
    pause > nul
    exit /b %ERRORLEVEL%
)
echo BUILD REUSSI !
echo.

echo *****************************************************
echo * OPERATION TERMINEE AVEC SUCCES                    *
echo *****************************************************
echo.
echo Le projet a été nettoyé et compilé avec succès!
echo.
echo Appuyez sur une touche pour quitter.
pause > nul
