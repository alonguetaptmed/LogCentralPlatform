
# Script de configuration locale pour LogCentralPlatform
Write-Host "Configuration de LogCentralPlatform en environnement local" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green

# 1. Vérification des prérequis
Write-Host "`n1. Vérification des prérequis..." -ForegroundColor Cyan

# Vérifier .NET SDK
try {
    $dotnetVersion = dotnet --version
    Write-Host "  .NET SDK installé: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "  ERREUR: .NET SDK n'est pas installé. Veuillez l'installer depuis https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

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

# 2. Restauration des packages
Write-Host "`n2. Restauration des packages NuGet..." -ForegroundColor Cyan
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: Impossible de restaurer les packages NuGet" -ForegroundColor Red
    exit 1
} else {
    Write-Host "  Packages restaurés avec succès" -ForegroundColor Green
}

# 3. Configuration de la base de données
Write-Host "`n3. Configuration de la base de données..." -ForegroundColor Cyan
Write-Host "  Veuillez entrer les informations de connexion à la base de données" -ForegroundColor Yellow

# Par défaut, utiliser les valeurs du fichier appsettings.json
$defaultServer = "localhost\EBP"
$defaultDatabase = "LogCentralPlatform"
$defaultUserId = "sa"
$defaultPassword = "@ebp78EBP"

$server = Read-Host "  Serveur SQL Server [$defaultServer]"
if ([string]::IsNullOrWhiteSpace($server)) { $server = $defaultServer }

$database = Read-Host "  Nom de la base de données [$defaultDatabase]"
if ([string]::IsNullOrWhiteSpace($database)) { $database = $defaultDatabase }

$userId = Read-Host "  Identifiant SQL [$defaultUserId]"
if ([string]::IsNullOrWhiteSpace($userId)) { $userId = $defaultUserId }

$password = Read-Host "  Mot de passe SQL [$defaultPassword]" -AsSecureString
if ($password.Length -eq 0) { 
    $password = $defaultPassword 
} else {
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
    $password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

$connectionString = "Server=$server;Database=$database;User Id=$userId;Password=$password;TrustServerCertificate=True"

Write-Host "  Application des migrations de base de données..." -ForegroundColor Yellow
Push-Location src\LogCentralPlatform.Api
$env:ConnectionStrings__DefaultConnection = $connectionString
dotnet ef database update
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: Impossible d'appliquer les migrations de base de données" -ForegroundColor Red
    Pop-Location
    exit 1
} else {
    Write-Host "  Base de données créée avec succès" -ForegroundColor Green
}
Pop-Location

# 4. Compilation du projet
Write-Host "`n4. Compilation du projet..." -ForegroundColor Cyan
dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERREUR: Impossible de compiler le projet" -ForegroundColor Red
    exit 1
} else {
    Write-Host "  Projet compilé avec succès" -ForegroundColor Green
}

# 5. Instructions pour lancer l'application
Write-Host "`n5. Lancement de l'application" -ForegroundColor Cyan
Write-Host "  Pour lancer l'API, exécutez la commande suivante dans un terminal:" -ForegroundColor Yellow
Write-Host "  cd src\LogCentralPlatform.Api" -ForegroundColor White
Write-Host "  dotnet run" -ForegroundColor White
Write-Host "  L'API sera accessible à https://localhost:5001" -ForegroundColor White

Write-Host "`n  Pour lancer l'interface Web, exécutez la commande suivante dans un autre terminal:" -ForegroundColor Yellow
Write-Host "  cd src\LogCentralPlatform.Web" -ForegroundColor White
Write-Host "  dotnet run" -ForegroundColor White
Write-Host "  L'interface Web sera accessible à https://localhost:5003" -ForegroundColor White

Write-Host "`nConfiguration locale terminée avec succès!" -ForegroundColor Green
