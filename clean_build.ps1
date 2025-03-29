
# Script pour nettoyer et reconstruire le projet
Write-Host "Nettoyage et reconstruction du projet LogCentralPlatform" -ForegroundColor Green
Write-Host "========================================================" -ForegroundColor Green

# Vérifier les prérequis
Write-Host "`nVérification de .NET SDK..." -ForegroundColor Cyan
try {
    $dotnetVersion = dotnet --version
    Write-Host "  .NET SDK installé: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "  ERREUR: .NET SDK n'est pas installé. Veuillez l'installer depuis https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

# Nettoyer les dossiers bin et obj
Write-Host "`nNettoyage des dossiers bin et obj..." -ForegroundColor Cyan
Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | ForEach-Object {
    Write-Host "  Suppression de $($_.FullName)" -ForegroundColor Yellow
    Remove-Item -Path $_.FullName -Recurse -Force
}
Write-Host "  Nettoyage terminé" -ForegroundColor Green

# Supprimer les packages NuGet du cache global
Write-Host "`nNettoyage du cache NuGet local pour le projet..." -ForegroundColor Cyan
dotnet nuget locals http-cache --clear
dotnet nuget locals global-packages --clear
dotnet nuget locals temp --clear
Write-Host "  Cache NuGet nettoyé" -ForegroundColor Green

# Restaurer les packages NuGet
Write-Host "`nRestauration des packages NuGet..." -ForegroundColor Cyan
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: Impossible de restaurer les packages NuGet" -ForegroundColor Red
    exit 1
} else {
    Write-Host "  Packages restaurés avec succès" -ForegroundColor Green
}

# Compiler le projet sans avertissements
Write-Host "`nCompilation du projet..." -ForegroundColor Cyan
dotnet build /p:WarningLevel=0
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: La compilation a échoué" -ForegroundColor Red
    exit 1
} else {
    Write-Host "  Compilation réussie" -ForegroundColor Green
}

Write-Host "`nNettoyage et reconstruction terminés avec succès!" -ForegroundColor Green
Write-Host "`nVous pouvez maintenant lancer le projet:" -ForegroundColor Cyan
Write-Host "1. Pour lancer l'API:" -ForegroundColor White
Write-Host "   cd src\LogCentralPlatform.Api" -ForegroundColor White
Write-Host "   dotnet run" -ForegroundColor White
Write-Host "2. Pour lancer l'interface Web:" -ForegroundColor White
Write-Host "   cd src\LogCentralPlatform.Web" -ForegroundColor White
Write-Host "   dotnet run" -ForegroundColor White
