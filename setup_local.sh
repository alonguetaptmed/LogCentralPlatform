
#!/bin/bash

# Script de configuration locale pour LogCentralPlatform (Linux/macOS)
echo -e "\e[32mConfiguration de LogCentralPlatform en environnement local\e[0m"
echo -e "\e[32m============================================================\e[0m"

# 1. Vérification des prérequis
echo -e "\n\e[36m1. Vérification des prérequis...\e[0m"

# Vérifier .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo -e "\e[31m  ERREUR: .NET SDK n'est pas installé. Veuillez l'installer depuis https://dotnet.microsoft.com/download\e[0m"
    exit 1
else
    dotnet_version=$(dotnet --version)
    echo -e "\e[32m  .NET SDK installé: $dotnet_version\e[0m"
fi

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

# 2. Restauration des packages
echo -e "\n\e[36m2. Restauration des packages NuGet...\e[0m"
dotnet restore
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: Impossible de restaurer les packages NuGet\e[0m"
    exit 1
else
    echo -e "\e[32m  Packages restaurés avec succès\e[0m"
fi

# 3. Configuration de la base de données
echo -e "\n\e[36m3. Configuration de la base de données...\e[0m"
echo -e "\e[33m  Veuillez entrer les informations de connexion à la base de données\e[0m"

# Par défaut, utiliser les valeurs du fichier appsettings.json
default_server="localhost\\EBP"
default_database="LogCentralPlatform"
default_user_id="sa"
default_password="@ebp78EBP"

read -p "  Serveur SQL Server [$default_server]: " server
server=${server:-$default_server}

read -p "  Nom de la base de données [$default_database]: " database
database=${database:-$default_database}

read -p "  Identifiant SQL [$default_user_id]: " user_id
user_id=${user_id:-$default_user_id}

read -sp "  Mot de passe SQL [$default_password]: " password
echo
password=${password:-$default_password}

connection_string="Server=$server;Database=$database;User Id=$user_id;Password=$password;TrustServerCertificate=True"

echo -e "\e[33m  Application des migrations de base de données...\e[0m"
pushd src/LogCentralPlatform.Api > /dev/null
export ConnectionStrings__DefaultConnection="$connection_string"
dotnet ef database update
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: Impossible d'appliquer les migrations de base de données\e[0m"
    popd > /dev/null
    exit 1
else
    echo -e "\e[32m  Base de données créée avec succès\e[0m"
fi
popd > /dev/null

# 4. Compilation du projet
echo -e "\n\e[36m4. Compilation du projet...\e[0m"
dotnet build
if [ $? -ne 0 ]; then
    echo -e "\e[31m  ERREUR: Impossible de compiler le projet\e[0m"
    exit 1
else
    echo -e "\e[32m  Projet compilé avec succès\e[0m"
fi

# 5. Instructions pour lancer l'application
echo -e "\n\e[36m5. Lancement de l'application\e[0m"
echo -e "\e[33m  Pour lancer l'API, exécutez la commande suivante dans un terminal:\e[0m"
echo -e "  cd src/LogCentralPlatform.Api"
echo -e "  dotnet run"
echo -e "  L'API sera accessible à https://localhost:5001"

echo -e "\n\e[33m  Pour lancer l'interface Web, exécutez la commande suivante dans un autre terminal:\e[0m"
echo -e "  cd src/LogCentralPlatform.Web"
echo -e "  dotnet run"
echo -e "  L'interface Web sera accessible à https://localhost:5003"

echo -e "\n\e[32mConfiguration locale terminée avec succès!\e[0m"
