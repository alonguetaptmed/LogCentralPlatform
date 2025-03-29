# Guide d'intégration pour les applications clientes

Ce guide explique comment intégrer vos applications avec LogCentralPlatform pour la gestion centralisée des logs.

## Introduction

LogCentralPlatform permet de centraliser la collecte, le stockage et l'analyse des logs de vos applications. L'intégration se fait principalement via une API REST, ce qui permet d'intégrer facilement des applications développées dans différents langages.

## Prérequis

Avant de commencer l'intégration, vous devez disposer de :

1. Une instance de LogCentralPlatform déployée et accessible
2. Un service enregistré dans la plateforme avec une clé API valide
3. Les autorisations nécessaires pour envoyer des logs

## Enregistrement d'un service

Avant de pouvoir envoyer des logs, vous devez enregistrer votre application comme un service dans LogCentralPlatform :

1. Connectez-vous à l'interface web de LogCentralPlatform
2. Naviguez vers la section "Services" et cliquez sur "Ajouter un service"
3. Remplissez les informations requises :
   - Nom du service (ex: "MonApplication")
   - Description (ex: "Application de gestion des commandes")
   - Version (ex: "1.0.0")
   - Type de service (ex: "Application Web", "Service Windows", etc.)
   - Client associé
   - Environnement (ex: "Production", "Testing", "Development")
4. Validez la création du service
5. Notez la clé API générée, vous en aurez besoin pour l'authentification

## Intégration via API REST

### Authentification

Toutes les requêtes vers l'API doivent inclure la clé API dans l'en-tête `X-API-Key` :

```
X-API-Key: votre-clé-api
```

### Envoi d'un log

Pour envoyer un log, effectuez une requête POST vers l'endpoint `/api/logs` :

#### Exemple en C#

```csharp
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LogCentralClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public LogCentralClient(string apiUrl, string apiKey)
    {
        _apiUrl = apiUrl;
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }

    public async Task<bool> SendLogAsync(
        string message,
        string level = "Information",
        string category = "",
        string exceptionDetails = null,
        string stackTrace = null,
        string correlationId = null,
        Dictionary<string, string> metadata = null)
    {
        try
        {
            var logEntry = new
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                Category = category,
                ExceptionDetails = exceptionDetails,
                StackTrace = stackTrace,
                CorrelationId = correlationId,
                ContextData = null,
                ContainsSensitiveData = false,
                Metadata = metadata
            };

            var content = new StringContent(
                JsonSerializer.Serialize(logEntry),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/logs", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

// Utilisation
var client = new LogCentralClient(
    "https://api.logcentralplatform.com",
    "votre-clé-api");

await client.SendLogAsync(
    "Traitement de la commande #12345 réussi",
    "Information",
    "OrderProcessing",
    correlationId: "ord-12345");
```

#### Exemple en Java

```java
import com.fasterxml.jackson.databind.ObjectMapper;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Instant;
import java.util.HashMap;
import java.util.Map;

public class LogCentralClient {
    private final HttpClient httpClient;
    private final String apiUrl;
    private final String apiKey;
    private final ObjectMapper objectMapper;

    public LogCentralClient(String apiUrl, String apiKey) {
        this.apiUrl = apiUrl;
        this.apiKey = apiKey;
        this.httpClient = HttpClient.newHttpClient();
        this.objectMapper = new ObjectMapper();
    }

    public boolean sendLog(
            String message,
            String level,
            String category,
            String exceptionDetails,
            String stackTrace,
            String correlationId,
            Map<String, String> metadata) throws Exception {
        
        Map<String, Object> logEntry = new HashMap<>();
        logEntry.put("Timestamp", Instant.now().toString());
        logEntry.put("Level", level);
        logEntry.put("Message", message);
        logEntry.put("Category", category);
        logEntry.put("ExceptionDetails", exceptionDetails);
        logEntry.put("StackTrace", stackTrace);
        logEntry.put("CorrelationId", correlationId);
        logEntry.put("ContainsSensitiveData", false);
        logEntry.put("Metadata", metadata);

        String requestBody = objectMapper.writeValueAsString(logEntry);
        
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(apiUrl + "/api/logs"))
                .header("Content-Type", "application/json")
                .header("X-API-Key", apiKey)
                .POST(HttpRequest.BodyPublishers.ofString(requestBody))
                .build();

        HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
        return response.statusCode() == 201;
    }
}

// Utilisation
LogCentralClient client = new LogCentralClient(
    "https://api.logcentralplatform.com", 
    "votre-clé-api");

Map<String, String> metadata = new HashMap<>();
metadata.put("orderId", "12345");
metadata.put("customerId", "C-789");

client.sendLog(
    "Traitement de la commande #12345 réussi",
    "Information",
    "OrderProcessing",
    null,
    null,
    "ord-12345",
    metadata);
```

#### Exemple en JavaScript

```javascript
class LogCentralClient {
  constructor(apiUrl, apiKey) {
    this.apiUrl = apiUrl;
    this.apiKey = apiKey;
  }

  async sendLog(message, level = 'Information', category = '', exceptionDetails = null, 
                stackTrace = null, correlationId = null, metadata = null) {
    try {
      const logEntry = {
        timestamp: new Date().toISOString(),
        level,
        message,
        category,
        exceptionDetails,
        stackTrace,
        correlationId,
        contextData: null,
        containsSensitiveData: false,
        metadata
      };

      const response = await fetch(`${this.apiUrl}/api/logs`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-API-Key': this.apiKey
        },
        body: JSON.stringify(logEntry)
      });

      return response.ok;
    } catch (error) {
      console.error('Failed to send log to LogCentralPlatform:', error);
      return false;
    }
  }
}

// Utilisation
const client = new LogCentralClient(
  'https://api.logcentralplatform.com',
  'votre-clé-api'
);

client.sendLog(
  'Traitement de la commande #12345 réussi',
  'Information',
  'OrderProcessing',
  null,
  null,
  'ord-12345',
  { orderId: '12345', customerId: 'C-789' }
);
```

#### Exemple en Python

```python
import requests
import json
from datetime import datetime

class LogCentralClient:
    def __init__(self, api_url, api_key):
        self.api_url = api_url
        self.api_key = api_key
        
    def send_log(self, message, level="Information", category="", exception_details=None, 
                 stack_trace=None, correlation_id=None, metadata=None):
        try:
            log_entry = {
                "timestamp": datetime.utcnow().isoformat(),
                "level": level,
                "message": message,
                "category": category,
                "exceptionDetails": exception_details,
                "stackTrace": stack_trace,
                "correlationId": correlation_id,
                "contextData": None,
                "containsSensitiveData": False,
                "metadata": metadata
            }
            
            headers = {
                "Content-Type": "application/json",
                "X-API-Key": self.api_key
            }
            
            response = requests.post(
                f"{self.api_url}/api/logs",
                headers=headers,
                data=json.dumps(log_entry)
            )
            
            return response.status_code == 201
        except Exception as e:
            print(f"Failed to send log to LogCentralPlatform: {e}")
            return False

# Utilisation
client = LogCentralClient(
    "https://api.logcentralplatform.com",
    "votre-clé-api"
)

client.send_log(
    "Traitement de la commande #12345 réussi",
    level="Information",
    category="OrderProcessing",
    correlation_id="ord-12345",
    metadata={"orderId": "12345", "customerId": "C-789"}
)
```

### Format des données de log

Le format JSON attendu pour un log est le suivant :

```json
{
  "timestamp": "2025-03-29T12:00:00Z",
  "level": "Information",
  "message": "Traitement de la commande #12345 réussi",
  "category": "OrderProcessing",
  "exceptionDetails": null,
  "stackTrace": null,
  "correlationId": "ord-12345",
  "contextData": null,
  "containsSensitiveData": false,
  "metadata": {
    "orderId": "12345",
    "customerId": "C-789"
  }
}
```

#### Champs obligatoires

- `timestamp` : Date et heure de l'événement au format ISO 8601
- `level` : Niveau de gravité du log (Trace, Debug, Information, Warning, Error, Critical)
- `message` : Message principal du log

#### Champs optionnels

- `category` : Catégorie ou composant à l'origine du log
- `exceptionDetails` : Détails de l'exception (si applicable)
- `stackTrace` : Trace de la pile d'appels (si applicable)
- `correlationId` : Identifiant pour lier plusieurs logs à un même événement
- `contextData` : Données contextuelles additionnelles au format JSON
- `containsSensitiveData` : Indique si le log contient des données sensibles à protéger
- `metadata` : Métadonnées personnalisées sous forme de paires clé-valeur

## Intégration avec des frameworks courants

### ASP.NET Core

Pour ASP.NET Core, vous pouvez créer un fournisseur de journalisation personnalisé :

```csharp
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LogCentralLoggerProvider : ILoggerProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;
    private readonly string _apiKey;
    private readonly string _serviceName;

    public LogCentralLoggerProvider(string apiUrl, string apiKey, string serviceName)
    {
        _apiUrl = apiUrl;
        _apiKey = apiKey;
        _serviceName = serviceName;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new LogCentralLogger(_httpClient, _apiUrl, _serviceName, categoryName);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private class LogCentralLogger : ILogger
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _serviceName;
        private readonly string _categoryName;

        public LogCentralLogger(HttpClient httpClient, string apiUrl, string serviceName, string categoryName)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
            _serviceName = serviceName;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
                               Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            Task.Run(async () =>
            {
                try
                {
                    string message = formatter(state, exception);
                    string level = MapLogLevel(logLevel);

                    var logEntry = new
                    {
                        Timestamp = DateTime.UtcNow,
                        Level = level,
                        Message = message,
                        Category = _categoryName,
                        ExceptionDetails = exception?.ToString(),
                        StackTrace = exception?.StackTrace,
                        CorrelationId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ContextData = null,
                        ContainsSensitiveData = false
                    };

                    var content = new StringContent(
                        JsonSerializer.Serialize(logEntry),
                        Encoding.UTF8,
                        "application/json");

                    await _httpClient.PostAsync($"{_apiUrl}/api/logs", content);
                }
                catch
                {
                    // Éviter les exceptions dans le logger
                }
            });
        }

        private string MapLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "Trace",
                LogLevel.Debug => "Debug",
                LogLevel.Information => "Information",
                LogLevel.Warning => "Warning",
                LogLevel.Error => "Error",
                LogLevel.Critical => "Critical",
                _ => "Information"
            };
        }
    }
}

// Enregistrement dans Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddLogging(builder =>
    {
        builder.AddProvider(new LogCentralLoggerProvider(
            "https://api.logcentralplatform.com",
            "votre-clé-api",
            "MonApplication"
        ));
    });
    // ...
}
```

### Log4j (Java)

Pour Java avec Log4j, vous pouvez créer un appender personnalisé :

```java
import org.apache.logging.log4j.core.Appender;
import org.apache.logging.log4j.core.Core;
import org.apache.logging.log4j.core.Filter;
import org.apache.logging.log4j.core.LogEvent;
import org.apache.logging.log4j.core.appender.AbstractAppender;
import org.apache.logging.log4j.core.config.plugins.Plugin;
import org.apache.logging.log4j.core.config.plugins.PluginAttribute;
import org.apache.logging.log4j.core.config.plugins.PluginElement;
import org.apache.logging.log4j.core.config.plugins.PluginFactory;

import java.io.Serializable;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Instant;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;

@Plugin(name = "LogCentral", category = Core.CATEGORY_NAME, elementType = Appender.ELEMENT_TYPE)
public class LogCentralAppender extends AbstractAppender {
    private final String apiUrl;
    private final String apiKey;
    private final String serviceName;
    private final HttpClient httpClient;
    private final ScheduledExecutorService executorService;
    private final ObjectMapper objectMapper;

    protected LogCentralAppender(String name, Filter filter, String apiUrl, String apiKey, String serviceName) {
        super(name, filter, null);
        this.apiUrl = apiUrl;
        this.apiKey = apiKey;
        this.serviceName = serviceName;
        this.httpClient = HttpClient.newHttpClient();
        this.executorService = Executors.newScheduledThreadPool(1);
        this.objectMapper = new ObjectMapper();
    }

    @PluginFactory
    public static LogCentralAppender createAppender(
            @PluginAttribute("name") String name,
            @PluginAttribute("apiUrl") String apiUrl,
            @PluginAttribute("apiKey") String apiKey,
            @PluginAttribute("serviceName") String serviceName,
            @PluginElement("Filter") Filter filter) {

        return new LogCentralAppender(name, filter, apiUrl, apiKey, serviceName);
    }

    @Override
    public void append(LogEvent event) {
        executorService.submit(() -> {
            try {
                String level = event.getLevel().name();
                String message = event.getMessage().getFormattedMessage();
                String category = event.getLoggerName();
                Throwable exception = event.getThrown();
                
                Map<String, Object> logEntry = new HashMap<>();
                logEntry.put("timestamp", Instant.now().toString());
                logEntry.put("level", level);
                logEntry.put("message", message);
                logEntry.put("category", category);
                logEntry.put("exceptionDetails", exception != null ? exception.toString() : null);
                logEntry.put("stackTrace", getStackTrace(exception));
                logEntry.put("containsSensitiveData", false);
                
                String requestBody = objectMapper.writeValueAsString(logEntry);
                
                HttpRequest request = HttpRequest.newBuilder()
                        .uri(URI.create(apiUrl + "/api/logs"))
                        .header("Content-Type", "application/json")
                        .header("X-API-Key", apiKey)
                        .POST(HttpRequest.BodyPublishers.ofString(requestBody))
                        .build();

                httpClient.send(request, HttpResponse.BodyHandlers.ofString());
            } catch (Exception e) {
                // Éviter les exceptions dans l'appender
            }
        });
    }
    
    private String getStackTrace(Throwable exception) {
        if (exception == null) {
            return null;
        }
        
        StringWriter sw = new StringWriter();
        PrintWriter pw = new PrintWriter(sw);
        exception.printStackTrace(pw);
        return sw.toString();
    }
}
```

### Winston (Node.js)

Pour Node.js avec Winston, vous pouvez créer un transport personnalisé :

```javascript
const winston = require('winston');
const fetch = require('node-fetch');

class LogCentralTransport extends winston.Transport {
  constructor(opts) {
    super(opts);
    this.name = 'logcentral';
    this.apiUrl = opts.apiUrl;
    this.apiKey = opts.apiKey;
    this.serviceName = opts.serviceName;
  }

  async log(info, callback) {
    try {
      const { level, message, ...meta } = info;
      
      const logEntry = {
        timestamp: new Date().toISOString(),
        level: level,
        message: message,
        category: meta.category || '',
        exceptionDetails: meta.error ? meta.error.toString() : null,
        stackTrace: meta.error ? meta.error.stack : null,
        correlationId: meta.correlationId || null,
        contextData: null,
        containsSensitiveData: false,
        metadata: meta
      };

      const response = await fetch(`${this.apiUrl}/api/logs`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-API-Key': this.apiKey
        },
        body: JSON.stringify(logEntry)
      });

      if (callback) {
        callback(null, response.ok);
      }
    } catch (error) {
      console.error('Error sending log to LogCentralPlatform:', error);
      if (callback) {
        callback(error);
      }
    }
  }
}

// Utilisation
const logger = winston.createLogger({
  transports: [
    new LogCentralTransport({
      apiUrl: 'https://api.logcentralplatform.com',
      apiKey: 'votre-clé-api',
      serviceName: 'MonApplication',
      level: 'info'
    })
  ]
});

logger.info('Traitement de la commande #12345 réussi', {
  category: 'OrderProcessing',
  correlationId: 'ord-12345',
  orderId: '12345',
  customerId: 'C-789'
});
```

## Bonnes pratiques

### Structuration des logs

Pour tirer le meilleur parti de LogCentralPlatform, suivez ces bonnes pratiques :

1. **Utilisez les niveaux de gravité de manière cohérente**
   - Trace : Informations très détaillées pour le débogage
   - Debug : Informations utiles au développement
   - Information : Événements normaux de l'application
   - Warning : Situations anormales mais non critiques
   - Error : Erreurs qui affectent une fonctionnalité spécifique
   - Critical : Erreurs qui affectent l'ensemble de l'application

2. **Structurez les messages de log**
   - Incluez des informations contextuelles utiles
   - Utilisez un format cohérent
   - Évitez les messages vagues comme "Erreur survenue"

3. **Utilisez les identifiants de corrélation**
   - Attribuez un ID unique à chaque transaction
   - Propagez cet ID à travers les différentes couches et services
   - Cela permettra de suivre le flux complet d'une transaction

4. **Exploitez les métadonnées**
   - Ajoutez des informations structurées qui pourront être filtrées et analysées
   - Par exemple : IDs utilisateur, IDs de transaction, données de performance

### Gestion des exceptions

Pour les logs d'erreur :

1. Capturez les détails complets de l'exception
2. Incluez la stack trace pour faciliter le débogage
3. Ajoutez du contexte sur ce qui s'est passé avant l'erreur
4. Utilisez le bon niveau de gravité (Error pour la plupart des exceptions, Critical pour les erreurs graves)

### Performance et fiabilité

Pour éviter d'impacter les performances de votre application :

1. **Logging asynchrone** : Envoyez les logs de manière asynchrone
2. **Buffer et batch** : Considérez l'envoi par lots pour les applications à haut volume
3. **Circuit breaker** : Implémentez un mécanisme de circuit breaker pour éviter de ralentir l'application si LogCentralPlatform est indisponible
4. **Stockage local** : Prévoyez un stockage temporaire local en cas d'indisponibilité de la plateforme

## Résolution des problèmes

### Problèmes courants

| Problème | Cause possible | Solution |
|----------|----------------|----------|
| Erreur 401 Unauthorized | Clé API invalide ou expirée | Vérifiez la clé API et régénérez-la si nécessaire |
| Erreur 403 Forbidden | Permissions insuffisantes | Vérifiez que le service a les permissions nécessaires |
| Erreur 429 Too Many Requests | Limite de débit dépassée | Implémentez un mécanisme de retry avec backoff exponentiel |
| Logs non visibles dans l'interface | Filtres incorrects | Vérifiez les filtres appliqués dans l'interface |
| Performance dégradée | Volume trop important | Considérez l'envoi par lots ou réduisez le niveau de verbosité |

### Test de l'intégration

Vous pouvez vérifier que votre intégration fonctionne correctement en :

1. Envoyant un log de test avec un message distinctif
2. Vérifiant dans l'interface de LogCentralPlatform que le log est bien reçu
3. Confirmant que tous les champs sont correctement remplis

## API Client pour autres langages

Des exemples d'intégration pour d'autres langages sont disponibles sur demande. Si vous avez besoin d'une intégration pour un langage non couvert dans ce guide, contactez l'équipe de support.

## Références

- [Documentation complète de l'API](../api-reference.md)
- [Guide de déploiement](../deployment/GUIDE_DEPLOIEMENT.md)
- [Guide de développement](../development/GUIDE_DEVELOPPEMENT.md)