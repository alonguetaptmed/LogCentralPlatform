
# Script pour générer un script SQL de création de base de données
Write-Host "Génération d'un script SQL pour la création de la base de données LogCentralPlatform" -ForegroundColor Green
Write-Host "===========================================================================" -ForegroundColor Green

# Vérifier les prérequis
Write-Host "`nVérification des prérequis..." -ForegroundColor Cyan

# Vérifier EF Core Tools
try {
    $efVersion = dotnet ef --version
    Write-Host "  Entity Framework Core Tools installé: $efVersion" -ForegroundColor Green
} catch {
    Write-Host "  Entity Framework Core Tools n'est pas installé. Installation en cours..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ERREUR: Impossible d'installer Entity Framework Core Tools" -ForegroundColor Red
        exit 1
    } else {
        Write-Host "  Entity Framework Core Tools installé avec succès" -ForegroundColor Green
    }
}

# Restaurer les packages
Write-Host "`nRestauration des packages NuGet..." -ForegroundColor Cyan
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: Impossible de restaurer les packages NuGet" -ForegroundColor Red
    exit 1
} else {
    Write-Host "  Packages restaurés avec succès" -ForegroundColor Green
}

# Générer le script SQL
Write-Host "`nGénération du script SQL..." -ForegroundColor Cyan
Push-Location src\LogCentralPlatform.Api
dotnet ef migrations script -o ..\..\database_creation_script.sql -i
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: Impossible de générer le script SQL" -ForegroundColor Red
    Pop-Location
    exit 1
} else {
    Write-Host "  Script SQL généré avec succès" -ForegroundColor Green
    Write-Host "  Le script a été sauvegardé dans : database_creation_script.sql" -ForegroundColor Green
}
Pop-Location

Write-Host "`nInstructions d'utilisation du script SQL :" -ForegroundColor Cyan
Write-Host "  1. Ouvrez SQL Server Management Studio" -ForegroundColor White
Write-Host "  2. Connectez-vous à votre instance SQL Server" -ForegroundColor White
Write-Host "  3. Créez une nouvelle base de données nommée 'LogCentralPlatform'" -ForegroundColor White
Write-Host "  4. Ouvrez le fichier 'database_creation_script.sql'" -ForegroundColor White
Write-Host "  5. Exécutez le script" -ForegroundColor White

Write-Host "`nGénération du script SQL terminée avec succès!" -ForegroundColor Green
