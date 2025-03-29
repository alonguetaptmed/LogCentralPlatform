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

### Configuration

1. Cloner le dépôt
```bash
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
```

2. Restaurer les packages NuGet
```bash
dotnet restore
```

3. Configurer la base de données
- Mettre à jour la chaîne de connexion dans les fichiers `appsettings.json`
- Appliquer les migrations
```bash
cd src/LogCentralPlatform.Api
dotnet ef database update
```

4. Configurer n8n pour l'analyse IA (optionnel)
- Installer n8n : [https://n8n.io/](https://n8n.io/)
- Configurer l'URL et la clé API dans `appsettings.json`

### Lancement

Pour lancer l'API :
```bash
cd src/LogCentralPlatform.Api
dotnet run
```

Pour lancer l'interface web :
```bash
cd src/LogCentralPlatform.Web
dotnet run
```

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

## Statut du projet

Ce projet est en phase de développement initial. Les fonctionnalités principales sont en cours d'implémentation.

## Licence

[MIT](LICENSE)

## Contact

Pour toute question ou suggestion, veuillez ouvrir une issue sur ce dépôt GitHub.