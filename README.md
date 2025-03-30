# LogCentralPlatform

Plateforme centralisée pour la gestion, l'analyse et la surveillance des logs d'applications en ASP.NET Core MVC.

## Description du projet

LogCentralPlatform est une solution complète de gestion de logs qui permet de centraliser, analyser et surveiller les logs des applications et services Windows déployés chez des clients. Cette plateforme facilite le débogage et le monitoring en temps réel, et intègre des fonctionnalités d'analyse IA pour aider à la détection d'anomalies et à la résolution de problèmes.

## Fonctionnalités principales

### Collecte et stockage des logs via API

- API REST sécurisée pour la transmission des logs en temps réel
- Authentification par clé API pour chaque service
- Stockage optimisé en base de données SQL Server avec indexation

### Suivi et gestion des services

- Interface utilisateur moderne pour visualiser l'état des services
- Intervalles de reporting configurables
- Alertes et notifications personnalisables

### Analyse IA avec n8n

- Détection automatique d'anomalies
- Suggestions de solutions basées sur l'analyse des logs
- Aide au débogage avec analyse du code source

### Sécurité et gestion des accès

- Authentification JWT et gestion des droits utilisateurs
- Chiffrement des données sensibles
- Audit et traçabilité des actions

## Architecture du projet

Le projet est organisé en plusieurs couches selon les principes de la Clean Architecture :

- `LogCentralPlatform.Core` : Entités, interfaces et logique métier
- `LogCentralPlatform.Infrastructure` : Implémentation de la persistance et services externes
- `LogCentralPlatform.Api` : API REST pour la réception des logs
- `LogCentralPlatform.Web` : Interface utilisateur web MVC

## Technologies utilisées

- **Backend** : ASP.NET Core 8.0, Entity Framework Core
- **Base de données** : SQL Server
- **Frontend** : ASP.NET MVC, Bootstrap
- **API** : REST, JWT pour l'authentification
- **IA** : n8n pour l'automatisation et l'analyse

## Installation et démarrage

### Prérequis

- .NET 8.0 SDK
- SQL Server (local ou distant)
- Visual Studio 2022 ou VS Code

### Configuration rapide (script automatisé)

Pour restaurer rapidement les packages et construire le projet :

1. Clonez le dépôt
```powershell
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
cd LogCentralPlatform
```

2. Exécutez le script de build
```powershell
.\build.bat
```

Ce script va automatiquement :
- Restaurer tous les packages NuGet
- Compiler la solution

### Configuration simplifiée (recommandée)

Nous fournissons des scripts pour faciliter la configuration locale:

#### Windows (PowerShell)

1. Clonez le dépôt
```powershell
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
cd LogCentralPlatform
```

2. Exécutez le script de configuration
```powershell
.\setup_local.ps1
```

Le script vous guidera à travers les étapes pour:
- Vérifier les prérequis
- Restaurer les packages
- Configurer la base de données
- Compiler le projet
- Lancer l'application

#### Linux/macOS (Bash)

1. Clonez le dépôt
```bash
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
cd LogCentralPlatform
```

2. Rendez le script exécutable et lancez-le
```bash
chmod +x setup_local.sh
./setup_local.sh
```

### Configuration manuelle

Si vous préférez configurer manuellement:

1. Clonez le dépôt
```bash
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
cd LogCentralPlatform
```

2. Restaurez les packages NuGet
```bash
dotnet restore
```

3. Configurez la base de données
   - Mettez à jour la chaîne de connexion dans les fichiers `src/LogCentralPlatform.Api/appsettings.json` et `src/LogCentralPlatform.Web/appsettings.json`
   - Appliquez les migrations
   ```bash
   cd src/LogCentralPlatform.Api
   dotnet ef database update
   ```

4. Compilez la solution
```bash
dotnet build
```

5. Lancez l'application
   - Pour l'API:
   ```bash
   cd src/LogCentralPlatform.Api
   dotnet run
   ```
   - Pour l'interface Web (dans un autre terminal):
   ```bash
   cd src/LogCentralPlatform.Web
   dotnet run
   ```

### Premier accès à l'application

Après avoir démarré l'API et l'interface Web:

1. Accédez à l'interface Web via votre navigateur à l'adresse https://localhost:5003
2. Créez un compte administrateur (lors du premier lancement)
3. Connectez-vous et commencez à configurer vos services et clients

## Intégration dans les applications clientes

Pour intégrer LogCentralPlatform dans une application cliente, vous pouvez utiliser le SDK client (à venir) ou effectuer des appels directs à l'API REST.

Exemple d'envoi d'un log via HTTP POST :
```csharp
using System.Net.Http;
using System.Text;
using System.Text.Json;

// Créer l'entrée de log
var logEntry = new 
{
    Timestamp = DateTime.UtcNow,
    Level = "Error",
    Message = "Une erreur est survenue",
    Category = "Database",
    StackTrace = "..."
};

// Convertir en JSON
var content = new StringContent(
    JsonSerializer.Serialize(logEntry),
    Encoding.UTF8,
    "application/json");

// Envoyer à l'API
using var client = new HttpClient();
client.DefaultRequestHeaders.Add("X-API-Key", "votre-clé-api");
var response = await client.PostAsync("https://votre-instance/api/logs", content);
```

## Résolution des problèmes courants

### Nettoyage complet du projet

Si vous rencontrez des problèmes de build persistants, nous fournissons des scripts pour nettoyer complètement le projet et reconstruire:

**Windows**:
```powershell
.\clean_build.ps1
```

**Linux/macOS**:
```bash
chmod +x clean_build.sh
./clean_build.sh
```

Ces scripts vont:
1. Supprimer tous les dossiers `bin` et `obj`
2. Vider le cache NuGet
3. Restaurer tous les packages
4. Reconstruire la solution

### Problèmes avec les packages NuGet

Si vous rencontrez des erreurs du type:
```
NETSDK1004 Le fichier de composants 'project.assets.json' est introuvable. Exécutez une restauration de package NuGet pour générer ce fichier.
```

Utilisez le script de build fourni :
```bash
.\build.bat
```

Ou manuellement :
```bash
dotnet restore
dotnet build
```

### Base de données

#### Problèmes avec Entity Framework Core Migrations

Si vous rencontrez des difficultés avec les migrations EF Core, vous pouvez générer un script SQL et l'exécuter directement:

**Windows**:
```powershell
.\generate_sql_script.ps1
```

**Linux/macOS**:
```bash
chmod +x generate_sql_script.sh
./generate_sql_script.sh
```

Ces scripts généreront un fichier `database_creation_script.sql` que vous pourrez exécuter directement dans SQL Server Management Studio ou via `sqlcmd`.

#### Autres problèmes de base de données

- **Erreur de connexion**: Vérifiez que SQL Server est en cours d'exécution et que la chaîne de connexion est correcte
- **Erreur "Login failed for user"**: Vérifiez les identifiants SQL Server et assurez-vous que l'authentification SQL est activée
- **Erreur "Cannot open database"**: Assurez-vous que la base de données existe ou que l'utilisateur a les droits pour la créer

### Erreurs de compilation

- **Erreurs d'ambiguïté de type (par exemple, `LogLevel`)**: Utilisez le nom complet du type, par exemple `LogCentralPlatform.Core.Entities.LogLevel`

- **ViewModels manquants**: Assurez-vous que tous les ViewModels sont correctement définis dans le dossier `src/LogCentralPlatform.Web/ViewModels`. Pour plus d'informations, consultez la [Référence des ViewModels](/docs/development/REFERENCE_VIEWMODELS.md).

- **Implémentations de méthodes manquantes**: Vérifiez que toutes les méthodes définies dans les interfaces sont correctement implémentées

### Application

- **Erreur de compilation**: Vérifiez que .NET 8.0 SDK est correctement installé
- **Erreur de port déjà utilisé**: Modifiez le port dans les propriétés de lancement ou arrêtez l'application qui utilise ce port

## Documentation

Une documentation plus détaillée est disponible dans le dossier `/docs`:
- [Documentation principale](/docs/README.md)
- [Guide de développement](/docs/development/GUIDE_DEVELOPPEMENT.md)
- [Référence des ViewModels](/docs/development/REFERENCE_VIEWMODELS.md)
- [Guide de déploiement](/docs/deployment/GUIDE_DEPLOIEMENT.md)
- [Guide d'intégration](/docs/integration/GUIDE_INTEGRATION.md)

## Statut du projet

Ce projet est en phase de développement initial. Les fonctionnalités principales sont en cours d'implémentation.

## Licence

[MIT](LICENSE)

## Contact

Pour toute question ou suggestion, veuillez ouvrir une issue sur ce dépôt GitHub.