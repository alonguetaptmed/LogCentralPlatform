# Documentation LogCentralPlatform

Bienvenue dans la documentation officielle de LogCentralPlatform, une solution complète de gestion centralisée des logs.

## Table des matières

1. [Vue d'ensemble](#vue-densemble)
2. [Architecture](#architecture)
3. [Installation](#installation)
4. [Configuration](#configuration)
5. [Utilisation](#utilisation)
6. [API Reference](#api-reference)
7. [Sécurité](#sécurité)
8. [Intégration n8n](#intégration-n8n)
9. [Développement](#développement)
10. [FAQ](#faq)

## Vue d'ensemble

LogCentralPlatform est une solution de gestion centralisée des logs qui permet de collecter, stocker, analyser et surveiller les logs provenant d'applications et services déployés chez les clients. Cette solution facilite le débogage et la maintenance proactive grâce à ses fonctionnalités d'analyse et d'alertes.

### Fonctionnalités principales

- Collecte centralisée des logs via API REST
- Interface de visualisation moderne et intuitive
- Analyse automatisée avec IA via n8n
- Alertes et notifications configurables
- Sécurité avancée avec authentification et autorisations granulaires

## Architecture

LogCentralPlatform est conçu selon les principes de la Clean Architecture, ce qui permet une séparation claire des responsabilités et facilite la maintenance et l'évolution du système.

### Composants

La solution est divisée en quatre projets principaux :

#### LogCentralPlatform.Core

Ce projet contient les entités de base, les interfaces et la logique métier indépendante des détails d'implémentation.

```
LogCentralPlatform.Core/
├── Entities/           # Modèles de domaine (LogEntry, RegisteredService, etc.)
├── Interfaces/         # Interfaces des repositories et services
└── Services/           # Services métier
```

#### LogCentralPlatform.Infrastructure

Ce projet contient l'implémentation des interfaces définies dans Core, notamment pour la persistance des données et les services externes.

```
LogCentralPlatform.Infrastructure/
├── Data/               # Contexte EF Core et configurations
├── Repositories/       # Implémentation des repositories
├── Services/           # Implémentation des services externes
└── DependencyInjection.cs  # Configuration des services
```

#### LogCentralPlatform.Api

API REST pour la collecte des logs et la gestion des services.

```
LogCentralPlatform.Api/
├── Controllers/        # Contrôleurs API
├── Models/             # DTOs et modèles de requête/réponse
└── Middleware/         # Middleware personnalisés
```

#### LogCentralPlatform.Web

Interface utilisateur pour la visualisation et la gestion des logs.

```
LogCentralPlatform.Web/
├── Controllers/        # Contrôleurs MVC
├── Models/             # ViewModels
└── Views/              # Vues Razor
```

### Diagramme d'architecture

```
[Applications Clientes] → [API REST] → [Core (Logique métier)] → [Infrastructure (Base de données, n8n)]
                                          ↑
                                          |
                                    [Interface Web]
```

## Installation

### Prérequis

- .NET 8.0 SDK
- SQL Server
- n8n (optionnel pour l'analyse IA)

### Installation depuis les sources

1. Clonez le dépôt

```bash
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
cd LogCentralPlatform
```

2. Restaurez les packages NuGet

```bash
dotnet restore
```

3. Créez la base de données

```bash
cd src/LogCentralPlatform.Api
dotnet ef database update
```

4. Lancez l'API

```bash
dotnet run
```

5. Dans un autre terminal, lancez l'interface Web

```bash
cd ../LogCentralPlatform.Web
dotnet run
```

## Configuration

### Configuration de la base de données

La configuration de la connexion à la base de données se fait dans les fichiers `appsettings.json` des projets Api et Web.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\EBP;Database=LogCentralPlatform;User Id=sa;Password=@ebp78EBP;TrustServerCertificate=True"
}
```

### Configuration de l'authentification

```json
"JwtSettings": {
  "SecretKey": "VOTRE_CLE_SECRETE_LONGUE_ET_COMPLEXE",
  "Issuer": "LogCentralPlatform",
  "Audience": "LogCentralPlatformClients",
  "ExpirationInMinutes": 60
}
```

### Configuration de n8n

```json
"AISettings": {
  "N8nApiUrl": "http://localhost:5678/webhook",
  "N8nApiKey": "votre_cle_api_n8n",
  "AnalysisCronSchedule": "0 */4 * * *",
  "MaxConcurrentAnalyses": 5
}
```

## Utilisation

### Enregistrement d'un service

Avant de pouvoir envoyer des logs, vous devez enregistrer votre service dans la plateforme :

1. Connectez-vous à l'interface Web
2. Accédez à la section "Services" et cliquez sur "Ajouter un service"
3. Remplissez les informations requises et enregistrez
4. Notez la clé API générée pour l'utiliser dans votre application

### Envoi de logs depuis une application

Pour envoyer des logs, utilisez la clé API obtenue et effectuez une requête POST vers l'API :

```csharp
// Exemple d'envoi de log en C#
using System.Net.Http;
using System.Text;
using System.Text.Json;

var logEntry = new
{
    Timestamp = DateTime.UtcNow,
    Level = "Error",
    Message = "Une erreur est survenue",
    Category = "Database",
    StackTrace = "..."
};

var content = new StringContent(
    JsonSerializer.Serialize(logEntry),
    Encoding.UTF8,
    "application/json");

using var client = new HttpClient();
client.DefaultRequestHeaders.Add("X-API-Key", "votre-clé-api");
var response = await client.PostAsync("https://votre-instance/api/logs", content);
```

### Visualisation des logs

1. Connectez-vous à l'interface Web
2. Accédez à la section "Logs"
3. Utilisez les filtres pour affiner votre recherche
4. Cliquez sur un log pour voir les détails

### Configuration des alertes

1. Connectez-vous à l'interface Web
2. Accédez à la section "Services"
3. Sélectionnez un service et cliquez sur "Configurer les alertes"
4. Définissez les conditions d'alerte et les destinataires

## API Reference

### Authentification

L'API utilise deux méthodes d'authentification :
- **API Key** : Pour les services qui envoient des logs
- **JWT Token** : Pour l'accès à l'API de gestion

#### API Key Authentication

Ajoutez l'en-tête `X-API-Key` à vos requêtes :

```
X-API-Key: votre-clé-api
```

#### JWT Authentication

Obtenez un token JWT en vous authentifiant, puis utilisez-le dans l'en-tête Authorization :

```
Authorization: Bearer votre-token-jwt
```

### Endpoints principaux

#### Logs

- `POST /api/logs` - Créer un nouveau log
- `GET /api/logs/{id}` - Obtenir un log par son ID
- `POST /api/logs/search` - Rechercher des logs
- `POST /api/logs/{id}/analyze` - Analyser un log avec l'IA

#### Services

- `GET /api/services` - Lister tous les services
- `GET /api/services/{id}` - Obtenir un service par son ID
- `POST /api/services` - Créer un nouveau service
- `PUT /api/services/{id}` - Mettre à jour un service
- `PATCH /api/services/{id}/activate` - Activer un service
- `PATCH /api/services/{id}/deactivate` - Désactiver un service
- `POST /api/services/{id}/regenerate-api-key` - Régénérer la clé API d'un service

## Sécurité

### Authentification et autorisation

LogCentralPlatform utilise JWT pour l'authentification des utilisateurs avec des rôles pour l'autorisation :
- **Admin** : Accès complet
- **Support** : Accès en lecture à tous les clients
- **Client** : Accès limité à ses propres services

### Sécurisation des données sensibles

- Les données sensibles dans les logs peuvent être marquées et chiffrées
- Les mots de passe sont stockés sous forme de hash avec salt
- Les clés API sont générées avec un niveau d'entropie élevé

## Intégration n8n

LogCentralPlatform s'intègre avec n8n pour l'analyse automatisée des logs. Cette intégration permet :
- Détection d'anomalies
- Suggestion de solutions
- Automatisation des alertes

### Configuration des workflows n8n

1. Installez n8n sur votre serveur
2. Importez les workflows prédéfinis
3. Configurez l'URL et la clé API dans les paramètres de LogCentralPlatform

## Développement

### Structure du code

Le projet suit les principes SOLID et utilise une architecture en couches :
- **Core** : Logique métier
- **Infrastructure** : Accès aux données et services externes
- **API** : Points d'entrée REST
- **Web** : Interface utilisateur

### Contribution

1. Forkez le dépôt
2. Créez une branche pour votre fonctionnalité
3. Soumettez une pull request

#### Style de code

Le projet suit les conventions de style de C# standard. Utilisez l'outil de formatage intégré de Visual Studio.

## FAQ

### Questions fréquentes

**Q: Comment puis-je intégrer la plateforme avec une application existante ?**

R: Vous pouvez utiliser notre API REST avec la clé API générée pour chaque service. Consultez la section [Envoi de logs depuis une application](#envoi-de-logs-depuis-une-application).

**Q: La plateforme supporte-t-elle les environnements multi-tenant ?**

R: Oui, chaque client peut avoir plusieurs services et les utilisateurs peuvent avoir accès à un ou plusieurs clients.

**Q: Comment fonctionne l'analyse IA des logs ?**

R: Nous utilisons n8n pour orchestrer l'analyse des logs. Les workflows analysent les patterns, détectent les anomalies et suggèrent des solutions basées sur l'historique des problèmes similaires.

---

Dernière mise à jour: 29 mars 2025