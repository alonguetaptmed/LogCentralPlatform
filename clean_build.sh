
#!/bin/bash

# Script pour nettoyer et reconstruire le projet (Linux/macOS)
echo -e "\e[32mNettoyage et reconstruction du projet LogCentralPlatform\e[0m"
echo -e "\e[32m========================================================\e[0m"

# Vérifier les prérequis
echo -e "\n\e[36mVérification de .NET SDK...\e[0m"
if ! command -v dotnet &> /dev/null; then
    echo -e "\e[31m  ERREUR: .NET SDK n'est pas installé. Veuillez l'installer depuis https://dotnet.microsoft.com/download\e[0m"
    exit 1
else
    dotnet_version=$(dotnet --version)
    echo -e "\e[32m  .NET SDK installé: $dotnet_version\e[0m"
fi

# Nettoyer les dossiers bin et obj
echo -e "\n\e[36mNettoyage des dossiers bin et obj...\e[0m"
find . -name bin -type d -exec rm -rf {} +
find . -name obj -type d -exec rm -rf {} +
echo -e "\e[32m  Nettoyage terminé\e[0m"

# Supprimer les packages NuGet du cache global
echo -e "\n\e[36mNettoyage du cache NuGet local pour le projet...\e[0m"
dotnet nuget locals http-cache --clear
dotnet nuget locals global-packages --clear
dotnet nuget locals temp --clear
echo -e "\e[32m  Cache NuGet nettoyé\e[0m"

# Restaurer les packages NuGet
echo -e "\n\e[36mRestauration des packages NuGet...\e[0m"
dotnet restore
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: Impossible de restaurer les packages NuGet\e[0m"
    exit 1
else
    echo -e "\e[32m  Packages restaurés avec succès\e[0m"
fi

# Compiler le projet sans avertissements
echo -e "\n\e[36mCompilation du projet...\e[0m"
dotnet build /p:WarningLevel=0
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: La compilation a échoué\e[0m"
    exit 1
else
    echo -e "\e[32m  Compilation réussie\e[0m"
fi

echo -e "\n\e[32mNettoyage et reconstruction terminés avec succès!\e[0m"
echo -e "\n\e[36mVous pouvez maintenant lancer le projet:\e[0m"
echo -e "1. Pour lancer l'API:"
echo -e "   cd src/LogCentralPlatform.Api"
echo -e "   dotnet run"
echo -e "2. Pour lancer l'interface Web:"
echo -e "   cd src/LogCentralPlatform.Web"
echo -e "   dotnet run"
