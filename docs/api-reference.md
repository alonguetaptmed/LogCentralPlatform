# API Reference

Cette documentation détaille toutes les API disponibles dans LogCentralPlatform.

## Authentification

L'API LogCentralPlatform prend en charge deux méthodes d'authentification :

### API Key Authentication

Utilisée par les services enregistrés pour envoyer des logs à la plateforme.

```
X-API-Key: votre-clé-api
```

### JWT Authentication

Utilisée pour l'accès aux API de gestion et d'administration.

```
Authorization: Bearer votre-token-jwt
```

## API Logs

### Créer un log

Enregistre un nouveau log dans le système.

**Endpoint**: `POST /api/logs`  
**Auth**: API Key (X-API-Key header)

**Requête**:
```json
{
  "timestamp": "2025-03-29T10:30:00Z",
  "level": "Error",
  "message": "Database connection failed",
  "category": "Database",
  "exceptionDetails": "System.Data.SqlClient.SqlException: Connection timeout",
  "stackTrace": "at DatabaseService.Connect() in DatabaseService.cs:line 42",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000",
  "contextData": "{\"attempt\": 3, \"server\": \"DB01\"}",
  "containsSensitiveData": false
}
```

**Réponse** (201 Created):
```json
{
  "id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "receivedAt": "2025-03-29T10:30:05Z",
  "success": true
}
```

### Récupérer un log

Récupère les détails d'un log spécifique par son ID.

**Endpoint**: `GET /api/logs/{id}`  
**Auth**: JWT Bearer Token

**Réponse** (200 OK):
```json
{
  "id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "timestamp": "2025-03-29T10:30:00Z",
  "level": "Error",
  "message": "Database connection failed",
  "serviceId": "89ed44f5-7cbd-4e5f-bd3a-c31c4a7d44c9",
  "serviceName": "PaymentProcessor",
  "serviceVersion": "1.2.3",
  "environment": "Production",
  "category": "Database",
  "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "clientName": "ACME Corp",
  "exceptionDetails": "System.Data.SqlClient.SqlException: Connection timeout",
  "stackTrace": "at DatabaseService.Connect() in DatabaseService.cs:line 42",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000",
  "contextData": "{\"attempt\": 3, \"server\": \"DB01\"}",
  "ipAddress": "192.168.1.100",
  "analyzedByAI": true,
  "aiAnalysisResult": "This error indicates a database connectivity issue. The connection timeout suggests network latency or database server overload.",
  "receivedAt": "2025-03-29T10:30:05Z"
}
```

### Rechercher des logs

Recherche des logs selon divers critères.

**Endpoint**: `POST /api/logs/search`  
**Auth**: JWT Bearer Token

**Requête**:
```json
{
  "startDate": "2025-03-22T00:00:00Z",
  "endDate": "2025-03-29T23:59:59Z",
  "serviceId": "89ed44f5-7cbd-4e5f-bd3a-c31c4a7d44c9",
  "clientId": null,
  "minLevel": "Warning",
  "searchText": "connection",
  "skip": 0,
  "take": 50
}
```

**Réponse** (200 OK):
```json
{
  "logs": [
    {
      "id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
      "timestamp": "2025-03-29T10:30:00Z",
      "level": "Error",
      "message": "Database connection failed",
      "serviceName": "PaymentProcessor",
      "serviceVersion": "1.2.3",
      "environment": "Production",
      "category": "Database",
      "clientName": "ACME Corp",
      "analyzedByAI": true,
      "receivedAt": "2025-03-29T10:30:05Z"
    },
    // ... autres logs
  ],
  "totalCount": 120,
  "success": true
}
```

### Analyser un log avec l'IA

Déclenche une analyse IA sur un log spécifique.

**Endpoint**: `POST /api/logs/{id}/analyze`  
**Auth**: JWT Bearer Token (Role: Admin,Support)

**Réponse** (200 OK):
```json
{
  "id": "a2b0d7e9-c871-4b9c-b1a3-f06dbe92ae5f",
  "analyzedAt": "2025-03-29T10:35:12Z",
  "summary": "Analysis of log f47ac10b-58cc-4372-a567-0e02b2c3d479 for service PaymentProcessor.\n\nThis error indicates a database connectivity issue. The connection timeout suggests network latency or database server overload. Similar errors have occurred 5 times in the last 24 hours.",
  "confidenceLevel": 85,
  "anomalies": [
    {
      "id": "d591ddc0-3616-4e8a-9a8c-bd9ce6526222",
      "type": "RecurringError",
      "description": "Database connection failures are occurring regularly",
      "severity": "Error",
      "occurrenceCount": 5,
      "firstOccurrence": "2025-03-28T22:15:43Z",
      "lastOccurrence": "2025-03-29T10:30:00Z"
    }
  ],
  "suggestions": [
    {
      "id": "7a9e8160-d9f3-4e25-9cd1-5b925feac371",
      "title": "Check Database Server Load",
      "description": "The database server appears to be experiencing high load or connectivity issues. Monitor server resources and check for network issues between the application and database servers.",
      "type": "Investigation",
      "confidenceLevel": 90
    },
    {
      "id": "3b11c96d-832e-4b1d-ab9b-8f4f7aa7256a",
      "title": "Implement Connection Retry Logic",
      "description": "Consider implementing a connection retry strategy with exponential backoff to handle transient connection issues.",
      "type": "CodeImprovement",
      "confidenceLevel": 85
    }
  ]
}
```

## API Services

### Lister tous les services

Récupère la liste de tous les services enregistrés.

**Endpoint**: `GET /api/services`  
**Auth**: JWT Bearer Token (Role: Admin,Support)  
**Paramètres Query**: `includeInactive=true` (optionnel)

**Réponse** (200 OK):
```json
[
  {
    "id": "89ed44f5-7cbd-4e5f-bd3a-c31c4a7d44c9",
    "name": "PaymentProcessor",
    "description": "Traite les paiements clients",
    "version": "1.2.3",
    "serviceType": "WindowsService",
    "apiKey": "api_key_redacted",
    "createdAt": "2025-01-15T08:30:00Z",
    "lastUpdatedAt": "2025-03-20T14:45:22Z",
    "lastLogReceivedAt": "2025-03-29T10:30:05Z",
    "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
    "clientName": "ACME Corp",
    "environment": "Production",
    "reportingIntervalMinutes": 15,
    "isActive": true,
    "isOnline": true,
    "alertsEnabled": true,
    "alertThreshold": "Error",
    "alertEmailRecipients": [
      "tech@acmecorp.com",
      "admin@example.com"
    ]
  },
  // ... autres services
]
```

### Obtenir un service

Récupère les détails d'un service spécifique.

**Endpoint**: `GET /api/services/{id}`  
**Auth**: JWT Bearer Token

**Réponse** (200 OK):
```json
{
  "id": "89ed44f5-7cbd-4e5f-bd3a-c31c4a7d44c9",
  "name": "PaymentProcessor",
  "description": "Traite les paiements clients",
  "version": "1.2.3",
  "serviceType": "WindowsService",
  "apiKey": "api_key_redacted",
  "createdAt": "2025-01-15T08:30:00Z",
  "lastUpdatedAt": "2025-03-20T14:45:22Z",
  "lastLogReceivedAt": "2025-03-29T10:30:05Z",
  "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "clientName": "ACME Corp",
  "environment": "Production",
  "reportingIntervalMinutes": 15,
  "isActive": true,
  "isOnline": true,
  "alertsEnabled": true,
  "alertThreshold": "Error",
  "alertEmailRecipients": [
    "tech@acmecorp.com",
    "admin@example.com"
  ],
  "webhookUrl": "https://hooks.acmecorp.com/notifications",
  "metadata": {
    "deploymentGroup": "financial",
    "region": "europe"
  },
  "sourceCodePath": "src/financial/PaymentProcessor"
}
```

### Créer un service

Enregistre un nouveau service dans le système.

**Endpoint**: `POST /api/services`  
**Auth**: JWT Bearer Token (Role: Admin)

**Requête**:
```json
{
  "name": "InventoryManager",
  "description": "Gère l'inventaire et les stocks",
  "version": "1.0.0",
  "serviceType": "WebApplication",
  "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "environment": "Production",
  "reportingIntervalMinutes": 30,
  "alertsEnabled": true,
  "alertThreshold": "Warning",
  "alertEmailRecipients": [
    "inventory@acmecorp.com",
    "admin@example.com"
  ],
  "metadata": {
    "deploymentGroup": "logistics",
    "region": "europe"
  }
}
```

**Réponse** (201 Created):
```json
{
  "id": "5d89f6d7-e9e7-4a63-9f3d-b4f1b3a7dc56",
  "name": "InventoryManager",
  "description": "Gère l'inventaire et les stocks",
  "version": "1.0.0",
  "serviceType": "WebApplication",
  "apiKey": "yHj2bZ8K9pL5qR7xW3vT1uA0cB4dE6fG",
  "createdAt": "2025-03-29T11:15:00Z",
  "lastUpdatedAt": "2025-03-29T11:15:00Z",
  "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "clientName": "ACME Corp",
  "environment": "Production",
  "reportingIntervalMinutes": 30,
  "isActive": true,
  "isOnline": false,
  "alertsEnabled": true,
  "alertThreshold": "Warning",
  "alertEmailRecipients": [
    "inventory@acmecorp.com",
    "admin@example.com"
  ],
  "metadata": {
    "deploymentGroup": "logistics",
    "region": "europe"
  }
}
```

### Mettre à jour un service

Met à jour les informations d'un service existant.

**Endpoint**: `PUT /api/services/{id}`  
**Auth**: JWT Bearer Token (Role: Admin)

**Requête**:
```json
{
  "name": "InventoryManager",
  "description": "Gère l'inventaire, les stocks et les commandes",
  "version": "1.0.1",
  "reportingIntervalMinutes": 15,
  "alertEmailRecipients": [
    "inventory@acmecorp.com",
    "logistics@acmecorp.com",
    "admin@example.com"
  ]
}
```

**Réponse** (200 OK):
```json
{
  "id": "5d89f6d7-e9e7-4a63-9f3d-b4f1b3a7dc56",
  "name": "InventoryManager",
  "description": "Gère l'inventaire, les stocks et les commandes",
  "version": "1.0.1",
  "serviceType": "WebApplication",
  "apiKey": "yHj2bZ8K9pL5qR7xW3vT1uA0cB4dE6fG",
  "createdAt": "2025-03-29T11:15:00Z",
  "lastUpdatedAt": "2025-03-29T11:20:15Z",
  "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "clientName": "ACME Corp",
  "environment": "Production",
  "reportingIntervalMinutes": 15,
  "isActive": true,
  "isOnline": false,
  "alertsEnabled": true,
  "alertThreshold": "Warning",
  "alertEmailRecipients": [
    "inventory@acmecorp.com",
    "logistics@acmecorp.com",
    "admin@example.com"
  ],
  "metadata": {
    "deploymentGroup": "logistics",
    "region": "europe"
  }
}
```

### Activer un service

Active un service précédemment désactivé.

**Endpoint**: `PATCH /api/services/{id}/activate`  
**Auth**: JWT Bearer Token (Role: Admin)

**Réponse** (200 OK):
```json
{
  "message": "Service activé avec succès."
}
```

### Désactiver un service

Désactive un service actif.

**Endpoint**: `PATCH /api/services/{id}/deactivate`  
**Auth**: JWT Bearer Token (Role: Admin)

**Réponse** (200 OK):
```json
{
  "message": "Service désactivé avec succès."
}
```

### Régénérer la clé API d'un service

Génère une nouvelle clé API pour un service et invalide l'ancienne.

**Endpoint**: `POST /api/services/{id}/regenerate-api-key`  
**Auth**: JWT Bearer Token (Role: Admin)

**Réponse** (200 OK):
```json
{
  "apiKey": "zJ3kP2mQ7nR5tS9xV1wY6uB4cD8fG0hI",
  "serviceId": "5d89f6d7-e9e7-4a63-9f3d-b4f1b3a7dc56",
  "success": true
}
```

### Rechercher des services

Recherche des services selon divers critères.

**Endpoint**: `POST /api/services/search`  
**Auth**: JWT Bearer Token (Role: Admin,Support)

**Requête**:
```json
{
  "searchTerm": "inventory",
  "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "includeInactive": false
}
```

**Réponse** (200 OK):
```json
[
  {
    "id": "5d89f6d7-e9e7-4a63-9f3d-b4f1b3a7dc56",
    "name": "InventoryManager",
    "description": "Gère l'inventaire, les stocks et les commandes",
    "version": "1.0.1",
    "serviceType": "WebApplication",
    "apiKey": "api_key_redacted",
    "createdAt": "2025-03-29T11:15:00Z",
    "lastUpdatedAt": "2025-03-29T11:20:15Z",
    "lastLogReceivedAt": null,
    "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
    "clientName": "ACME Corp",
    "environment": "Production",
    "reportingIntervalMinutes": 15,
    "isActive": true,
    "isOnline": false,
    "alertsEnabled": true,
    "alertThreshold": "Warning"
  }
]
```

### Récupérer les services hors ligne

Récupère la liste des services qui n'ont pas envoyé de logs récemment.

**Endpoint**: `GET /api/services/offline`  
**Auth**: JWT Bearer Token (Role: Admin,Support)

**Réponse** (200 OK):
```json
[
  {
    "id": "5d89f6d7-e9e7-4a63-9f3d-b4f1b3a7dc56",
    "name": "InventoryManager",
    "description": "Gère l'inventaire, les stocks et les commandes",
    "version": "1.0.1",
    "serviceType": "WebApplication",
    "lastLogReceivedAt": null,
    "clientId": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
    "clientName": "ACME Corp",
    "environment": "Production",
    "reportingIntervalMinutes": 15,
    "isActive": true,
    "isOnline": false
  },
  // ... autres services hors ligne
]
```

## API Clients

### Lister tous les clients

Récupère la liste de tous les clients.

**Endpoint**: `GET /api/clients`  
**Auth**: JWT Bearer Token (Role: Admin,Support)  
**Paramètres Query**: `includeInactive=true` (optionnel)

**Réponse** (200 OK):
```json
[
  {
    "id": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
    "name": "ACME Corp",
    "clientNumber": "CLT-001",
    "description": "Entreprise spécialisée dans les produits innovants",
    "email": "contact@acmecorp.com",
    "phone": "+33123456789",
    "createdAt": "2024-12-10T09:00:00Z",
    "lastUpdatedAt": "2025-02-15T14:30:22Z",
    "isActive": true
  },
  // ... autres clients
]
```

### Obtenir un client

Récupère les détails d'un client spécifique.

**Endpoint**: `GET /api/clients/{id}`  
**Auth**: JWT Bearer Token

**Réponse** (200 OK):
```json
{
  "id": "12e68d2b-8907-44a8-8bee-af8c47b056dd",
  "name": "ACME Corp",
  "clientNumber": "CLT-001",
  "description": "Entreprise spécialisée dans les produits innovants",
  "email": "contact@acmecorp.com",
  "phone": "+33123456789",
  "address": "123 Innovation Street, 75001 Paris, France",
  "createdAt": "2024-12-10T09:00:00Z",
  "lastUpdatedAt": "2025-02-15T14:30:22Z",
  "isActive": true,
  "contacts": [
    {
      "id": "3f7e9d8c-6b5a-4321-0123-456789abcdef",
      "name": "Jean Dupont",
      "role": "CTO",
      "email": "jean.dupont@acmecorp.com",
      "phone": "+33123456780",
      "receiveAlerts": true
    },
    {
      "id": "2a1b3c4d-5e6f-7890-a1b2-c3d4e5f67890",
      "name": "Marie Martin",
      "role": "IT Manager",
      "email": "marie.martin@acmecorp.com",
      "phone": "+33123456781",
      "receiveAlerts": true
    }
  ],
  "notificationSettings": {
    "emailNotificationsEnabled": true,
    "smsNotificationsEnabled": false,
    "webhookNotificationsEnabled": true,
    "webhookUrl": "https://hooks.acmecorp.com/notifications",
    "notificationThreshold": "Error"
  }
}
```

### Récupérer les services d'un client

Récupère la liste des services associés à un client spécifique.

**Endpoint**: `GET /api/clients/{id}/services`  
**Auth**: JWT Bearer Token

**Réponse** (200 OK):
```json
[
  {
    "id": "89ed44f5-7cbd-4e5f-bd3a-c31c4a7d44c9",
    "name": "PaymentProcessor",
    "description": "Traite les paiements clients",
    "version": "1.2.3",
    "serviceType": "WindowsService",
    "environment": "Production",
    "isActive": true,
    "isOnline": true
  },
  {
    "id": "5d89f6d7-e9e7-4a63-9f3d-b4f1b3a7dc56",
    "name": "InventoryManager",
    "description": "Gère l'inventaire, les stocks et les commandes",
    "version": "1.0.1",
    "serviceType": "WebApplication",
    "environment": "Production",
    "isActive": true,
    "isOnline": false
  }
  // ... autres services du client
]
```

## API Authentication

### Login

Authentifie un utilisateur et génère un token JWT.

**Endpoint**: `POST /api/auth/login`  
**Auth**: Aucune

**Requête**:
```json
{
  "username": "admin@example.com",
  "password": "votre_mot_de_passe"
}
```

**Réponse** (200 OK):
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-03-30T11:30:00Z",
  "roles": ["Admin"],
  "userId": "98765432-abcd-ef01-2345-6789abcdef01"
}
```

### Refresh Token

Rafraîchit un token JWT existant.

**Endpoint**: `POST /api/auth/refresh`  
**Auth**: JWT Bearer Token

**Réponse** (200 OK):
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-03-30T12:30:00Z"
}
```

### Vérifier le token

Vérifie la validité d'un token JWT.

**Endpoint**: `POST /api/auth/verify`  
**Auth**: JWT Bearer Token

**Réponse** (200 OK):
```json
{
  "isValid": true,
  "userId": "98765432-abcd-ef01-2345-6789abcdef01",
  "roles": ["Admin"]
}
```

## API IA

### Obtenir un rapport d'analyse

Génère un rapport d'analyse pour un service sur une période donnée.

**Endpoint**: `GET /api/ai/report/{serviceId}`  
**Auth**: JWT Bearer Token (Role: Admin,Support)  
**Paramètres Query**: `startDate=2025-03-01T00:00:00Z&endDate=2025-03-29T23:59:59Z`

**Réponse** (200 OK):
```json
{
  "id": "0a1b2c3d-4e5f-6789-0a1b-2c3d4e5f6789",
  "serviceId": "89ed44f5-7cbd-4e5f-bd3a-c31c4a7d44c9",
  "generatedAt": "2025-03-29T12:00:00Z",
  "startDate": "2025-03-01T00:00:00Z",
  "endDate": "2025-03-29T23:59:59Z",
  "executiveSummary": "Le service PaymentProcessor a enregistré 245 logs sur la période, dont 15 erreurs et 30 avertissements. Les principaux problèmes concernent des timeouts de connexion à la base de données qui surviennent généralement entre 2h et 4h du matin.",
  "logLevelStats": {
    "Critical": 2,
    "Error": 13,
    "Warning": 30,
    "Information": 150,
    "Debug": 50
  },
  "anomalies": [
    {
      "id": "d591ddc0-3616-4e8a-9a8c-bd9ce6526222",
      "type": "RecurringError",
      "description": "Database connection failures are occurring regularly during maintenance window",
      "severity": "Error",
      "occurrenceCount": 10,
      "firstOccurrence": "2025-03-02T02:15:43Z",
      "lastOccurrence": "2025-03-29T03:30:00Z"
    }
  ],
  "suggestions": [
    {
      "id": "7a9e8160-d9f3-4e25-9cd1-5b925feac371",
      "title": "Adjust Database Connection Timeout",
      "description": "Increase the database connection timeout during the maintenance window or implement a more robust retry strategy.",
      "type": "Configuration",
      "confidenceLevel": 90
    }
  ],
  "trends": [
    "Les erreurs se produisent principalement pendant la fenêtre de maintenance de la base de données",
    "Le taux d'erreur a diminué de 15% depuis la dernière mise à jour du service"
  ],
  "performanceAnalysis": "Le service maintient des temps de réponse satisfaisants en dehors des périodes de maintenance. Aucune dégradation progressive des performances n'a été détectée."
}
```

### Détecter des anomalies

Détecte des anomalies dans les logs d'un service sur une période donnée.

**Endpoint**: `GET /api/ai/anomalies/{serviceId}`  
**Auth**: JWT Bearer Token (Role: Admin,Support)  
**Paramètres Query**: `startDate=2025-03-01T00:00:00Z&endDate=2025-03-29T23:59:59Z`

**Réponse** (200 OK):
```json
[
  {
    "id": "d591ddc0-3616-4e8a-9a8c-bd9ce6526222",
    "type": "RecurringError",
    "description": "Database connection failures are occurring regularly during maintenance window",
    "severity": "Error",
    "occurrenceCount": 10,
    "firstOccurrence": "2025-03-02T02:15:43Z",
    "lastOccurrence": "2025-03-29T03:30:00Z"
  },
  {
    "id": "8c9d0e1f-2a3b-4c5d-6e7f-8a9b0c1d2e3f",
    "type": "SuddenIncrease",
    "description": "Sudden increase in warning logs on March 15th",
    "severity": "Warning",
    "occurrenceCount": 15,
    "firstOccurrence": "2025-03-15T10:00:00Z",
    "lastOccurrence": "2025-03-15T18:30:00Z"
  }
]
```

## Codes d'erreur

L'API peut retourner les codes d'erreur HTTP suivants :

- `200 OK` - Requête traitée avec succès
- `201 Created` - Ressource créée avec succès
- `400 Bad Request` - Requête invalide (paramètres manquants ou invalides)
- `401 Unauthorized` - Authentification requise ou échouée
- `403 Forbidden` - Authentifié mais non autorisé à accéder à la ressource
- `404 Not Found` - Ressource non trouvée
- `409 Conflict` - Conflit lors de la création ou mise à jour (ex: doublon)
- `429 Too Many Requests` - Trop de requêtes (rate limiting)
- `500 Internal Server Error` - Erreur interne du serveur

## Limites et throttling

L'API applique les limites suivantes :

- Maximum 100 requêtes par minute par IP
- Maximum 1000 requêtes par heure par client
- Taille maximale des logs : 1 MB
- Taille maximale des requêtes : 10 MB

## Versioning

L'API utilise un versionnement via l'URL. La version actuelle est v1.

Exemple : `https://api.logcentralplatform.com/v1/logs`

## Changelog

### v1.0.0 (29 mars 2025)
- Version initiale de l'API