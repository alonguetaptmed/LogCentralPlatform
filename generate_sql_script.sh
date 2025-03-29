
#!/bin/bash

# Script pour générer un script SQL de création de base de données (Linux/macOS)
echo -e "\e[32mGénération d'un script SQL pour la création de la base de données LogCentralPlatform\e[0m"
echo -e "\e[32m===========================================================================\e[0m"

# Vérifier les prérequis
echo -e "\n\e[36mVérification des prérequis...\e[0m"

# Vérifier EF Core Tools
if ! dotnet ef --version &> /dev/null; then
    echo -e "\e[33m  Entity Framework Core Tools n'est pas installé. Installation en cours...\e[0m"
    dotnet tool install --global dotnet-ef
    
    if [ $? -ne 0 ]; then
        echo -e "\e[31m  ERREUR: Impossible d'installer Entity Framework Core Tools\e[0m"
        exit 1
    else
        echo -e "\e[32m  Entity Framework Core Tools installé avec succès\e[0m"
    fi
else
    ef_version=$(dotnet ef --version)
    echo -e "\e[32m  Entity Framework Core Tools installé: $ef_version\e[0m"
fi

# Restaurer les packages
echo -e "\n\e[36mRestauration des packages NuGet...\e[0m"
dotnet restore
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: Impossible de restaurer les packages NuGet\e[0m"
    exit 1
else
    echo -e "\e[32m  Packages restaurés avec succès\e[0m"
fi

# Générer le script SQL
echo -e "\n\e[36mGénération du script SQL...\e[0m"
pushd src/LogCentralPlatform.Api > /dev/null
dotnet ef migrations script -o ../../database_creation_script.sql -i
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: Impossible de générer le script SQL\e[0m"
    popd > /dev/null
    exit 1
else
    echo -e "\e[32m  Script SQL généré avec succès\e[0m"
    echo -e "\e[32m  Le script a été sauvegardé dans : database_creation_script.sql\e[0m"
fi
popd > /dev/null

echo -e "\n\e[36mInstructions d'utilisation du script SQL :\e[0m"
echo -e "  1. Connectez-vous à votre instance SQL Server"
echo -e "  2. Créez une nouvelle base de données nommée 'LogCentralPlatform'"
echo -e "  3. Exécutez le script SQL avec la commande suivante :"
echo -e "     sqlcmd -S <serveur> -U <utilisateur> -P <mot_de_passe> -d LogCentralPlatform -i database_creation_script.sql"
echo -e "  4. Vous pouvez également utiliser un outil graphique comme Azure Data Studio ou DBeaver"

echo -e "\n\e[32mGénération du script SQL terminée avec succès!\e[0m"
