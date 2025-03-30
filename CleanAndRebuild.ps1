$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $scriptPath

Write-Host ""
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host "     NETTOYAGE ET RECONSTRUCTION COMPLETE        " -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host ""

# Étape 1: Suppression des dossiers bin et obj
Write-Host "[1/5] Suppression des dossiers bin et obj..." -ForegroundColor Yellow
$binObjFolders = Get-ChildItem -Path $scriptPath -Include bin, obj -Directory -Recurse
$binObjCount = $binObjFolders.Count

if ($binObjCount -gt 0) {
    foreach ($folder in $binObjFolders) {
        Remove-Item -Path $folder.FullName -Recurse -Force
        Write-Host "  - Supprimé: $($folder.FullName)" -ForegroundColor Gray
    }
    Write-Host "  $binObjCount dossiers supprimés avec succès." -ForegroundColor Green
}
else {
    Write-Host "  Aucun dossier bin ou obj trouvé." -ForegroundColor Gray
}
Write-Host ""

# Étape 2: Nettoyage du cache de Visual Studio
Write-Host "[2/5] Nettoyage du cache de Visual Studio..." -ForegroundColor Yellow
if (Test-Path "$env:USERPROFILE\.nuget\packages") {
    # Suppression sélective du cache NuGet
    if (Test-Path "$env:USERPROFILE\.nuget\packages\logcentralplatform*") {
        Remove-Item -Path "$env:USERPROFILE\.nuget\packages\logcentralplatform*" -Recurse -Force
        Write-Host "  Packages LogCentralPlatform supprimés du cache NuGet." -ForegroundColor Green
    }
    else {
        Write-Host "  Aucun package LogCentralPlatform trouvé dans le cache." -ForegroundColor Gray
    }
}

# Nettoyage du cache NuGet local
try {
    Write-Host "  Nettoyage du cache NuGet..." -ForegroundColor Gray
    dotnet nuget locals all --clear | Out-Null
    Write-Host "  Cache NuGet nettoyé avec succès." -ForegroundColor Green
}
catch {
    Write-Host "  Erreur lors du nettoyage du cache NuGet: $_" -ForegroundColor Red
}
Write-Host ""

# Étape 3: Restauration des packages NuGet
Write-Host "[3/5] Restauration des packages NuGet..." -ForegroundColor Yellow
try {
    dotnet restore
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Packages NuGet restaurés avec succès." -ForegroundColor Green
    }
    else {
        Write-Host "  Erreur lors de la restauration des packages NuGet (code: $LASTEXITCODE)" -ForegroundColor Red
        Write-Host "  Tentative de restauration avec --force..." -ForegroundColor Yellow
        dotnet restore --force
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  Restauration forcée réussie." -ForegroundColor Green
        }
        else {
            Write-Host "  Échec de la restauration. Veuillez vérifier les erreurs ci-dessus." -ForegroundColor Red
            exit 1
        }
    }
}
catch {
    Write-Host "  Erreur lors de la restauration des packages: $_" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Étape 4: Compilation de la solution
Write-Host "[4/5] Compilation de la solution..." -ForegroundColor Yellow
try {
    dotnet build
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Solution compilée avec succès." -ForegroundColor Green
    }
    else {
        Write-Host "  Erreur lors de la compilation (code: $LASTEXITCODE)" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host "  Erreur lors de la compilation: $_" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Étape 5: Vérification des erreurs restantes
Write-Host "[5/5] Vérification des erreurs restantes..." -ForegroundColor Yellow
$buildLog = dotnet build --nologo --verbosity quiet
$errorCount = ($buildLog | Select-String -Pattern "erreur" -SimpleMatch).Count

if ($errorCount -gt 0) {
    Write-Host "  $errorCount erreurs restantes dans le projet." -ForegroundColor Red
    Write-Host "  Voici les erreurs détectées:" -ForegroundColor Red
    $buildLog | Select-String -Pattern "erreur" -SimpleMatch | ForEach-Object {
        Write-Host "  - $_" -ForegroundColor Red
    }
}
else {
    Write-Host "  Aucune erreur détectée dans le projet." -ForegroundColor Green
}
Write-Host ""

Write-Host "=================================================" -ForegroundColor Cyan
Write-Host "     OPÉRATION TERMINÉE                          " -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host ""
