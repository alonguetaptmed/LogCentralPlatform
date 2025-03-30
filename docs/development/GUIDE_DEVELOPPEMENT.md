# Guide de développement

Ce guide fournit les informations essentielles pour les développeurs travaillant sur le projet LogCentralPlatform.

## Configuration de l'environnement de développement

### Prérequis

- Visual Studio 2022 ou plus récent (ou VS Code avec les extensions C#)
- .NET 8.0 SDK
- SQL Server (local ou distant)
- Git
- Node.js (pour n8n si utilisé pour l'analyse IA)

### Installation des outils

1. Installez [Visual Studio 2022](https://visualstudio.microsoft.com/fr/vs/)
   - Assurez-vous de sélectionner la charge de travail "Développement web et ASP.NET"
   
2. Installez [SQL Server Express](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads) ou utilisez une instance existante

3. (Optionnel) Installez [n8n](https://n8n.io/) pour le développement de workflows d'IA
   ```bash
   npm install n8n -g
   ```

### Clonage du projet

```bash
git clone https://github.com/alonguetaptmed/LogCentralPlatform.git
cd LogCentralPlatform
```

### Restauration des packages et compilation

```bash
dotnet restore
dotnet build
```

### Configuration de la base de données

1. Ouvrez le fichier `src/LogCentralPlatform.Api/appsettings.Development.json`
2. Modifiez la chaîne de connexion selon votre environnement :
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\EBP;Database=LogCentralPlatform_Dev;User Id=sa;Password=@ebp78EBP;TrustServerCertificate=True"
   }
   ```
3. Exécutez les migrations pour créer la base de données :
   ```bash
   cd src/LogCentralPlatform.Api
   dotnet ef database update
   ```

### Lancement du projet

1. Démarrez l'API :
   ```bash
   cd src/LogCentralPlatform.Api
   dotnet run
   ```

2. Dans un autre terminal, démarrez l'application Web :
   ```bash
   cd src/LogCentralPlatform.Web
   dotnet run
   ```

L'API sera accessible à https://localhost:5001 et l'interface Web à https://localhost:5003.

## Architecture du projet

Le projet suit une architecture en couches inspirée de la Clean Architecture :

1. **Core** : Contient les entités, interfaces et la logique métier
2. **Infrastructure** : Implémente les interfaces définies dans Core
3. **API** : Expose les fonctionnalités via une API REST
4. **Web** : Interface utilisateur MVC

### Structure des dossiers

```
LogCentralPlatform/
├── src/
│   ├── LogCentralPlatform.Core/             # Couche domaine
│   │   ├── Entities/                        # Modèles de domaine
│   │   └── Interfaces/                      # Interfaces des services
│   ├── LogCentralPlatform.Infrastructure/   # Implémentations
│   │   ├── Data/                            # Accès aux données
│   │   ├── Repositories/                    # Implémentation des repos
│   │   └── Services/                        # Implémentation des services
│   ├── LogCentralPlatform.Api/              # API REST
│   │   ├── Controllers/                     # Contrôleurs API
│   │   └── Models/                          # DTOs
│   └── LogCentralPlatform.Web/              # Interface Web
│       ├── Controllers/                     # Contrôleurs MVC
│       ├── ViewModels/                      # ViewModels pour les vues
│       ├── Models/                          # Autres modèles
│       └── Views/                           # Vues Razor
└── docs/                                    # Documentation
```

## ViewModels

Les ViewModels sont utilisés pour transmettre les données des contrôleurs aux vues. Ils sont organisés dans le dossier `src/LogCentralPlatform.Web/ViewModels/`.

### Types de ViewModels

1. **Modèles d'affichage**
   - `ClientListViewModel`, `ServiceListViewModel` : Pour les pages de liste
   - `ClientDetailsViewModel`, `ServiceDetailsViewModel` : Pour les pages de détail

2. **Modèles de formulaire**
   - `LoginViewModel` : Pour l'authentification
   - `SettingsViewModel` : Pour la configuration de l'application

3. **Modèles partagés**
   - `AlertViewModel` : Pour les messages d'alerte
   - `StatCardViewModel` : Pour les cartes de statistiques
   - `PaginationViewModel` : Pour la pagination

### Conventions pour les ViewModels

- Utilisez les annotations de validation pour les propriétés des formulaires
- Regroupez les ViewModels liés dans des fichiers logiques (ex: `ClientViewModels.cs`)
- Utilisez des classes imbriquées pour les petits modèles auxiliaires
- Assurez-vous d'initialiser les collections (ex: `= new List<T>()`)

### Exemple d'utilisation

```csharp
// Dans le contrôleur
public IActionResult Details(int id)
{
    var client = _clientService.GetClientById(id);
    
    var viewModel = new ClientDetailsViewModel
    {
        Id = client.Id,
        Name = client.Name,
        // ... autres propriétés
        Services = client.Services.Select(s => new ServiceSummary
        {
            Id = s.Id,
            Name = s.Name,
            // ... autres propriétés
        }).ToList()
    };
    
    return View(viewModel);
}

// Dans la vue (Razor)
@model LogCentralPlatform.Web.ViewModels.ClientDetailsViewModel

<h1>@Model.Name</h1>
// ... reste de la vue
```

## Conventions de codage

Nous suivons les [conventions de codage Microsoft](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) avec quelques spécificités :

### Nommage

- **Classes, méthodes, propriétés** : PascalCase
- **Variables locales, paramètres** : camelCase
- **Interfaces** : Préfixées par "I" (ex: ILogRepository)
- **Constantes** : SNAKE_CASE_MAJUSCULE

### Organisation du code

- Limitez chaque fichier à une seule classe (sauf pour les classes internes étroitement liées)
- Organisez le code dans l'ordre suivant :
  1. Champs privés
  2. Constructeurs
  3. Propriétés
  4. Méthodes publiques
  5. Méthodes privées

### Documentation du code

Utilisez les commentaires XML pour documenter toutes les classes et méthodes publiques :

```csharp
/// <summary>
/// Description de la classe ou de la méthode.
/// </summary>
/// <param name="paramName">Description du paramètre.</param>
/// <returns>Description de la valeur de retour.</returns>
public ReturnType MethodName(ParameterType paramName)
{
    // Implémentation
}
```

## Workflow de développement

### Gestion des branches

Nous utilisons une approche inspirée de GitFlow :

- `main` : Code en production
- `develop` : Branche d'intégration principale
- `feature/nom-fonctionnalité` : Branches de fonctionnalités
- `bugfix/nom-bug` : Corrections de bugs
- `release/version` : Préparation des releases

### Processus de contribution

1. Créez une branche à partir de `develop` : `feature/nom-fonctionnalité`
2. Développez et testez votre fonctionnalité
3. Assurez-vous que tous les tests passent
4. Mettez à jour la documentation si nécessaire
5. Soumettez une pull request vers `develop`
6. Après revue et approbation, la PR sera fusionnée

### Gestion des versions

Nous utilisons le [versionnement sémantique](https://semver.org/) (MAJOR.MINOR.PATCH) :

- MAJOR : Changements incompatibles avec les versions précédentes
- MINOR : Ajout de fonctionnalités rétrocompatibles
- PATCH : Corrections de bugs rétrocompatibles

## Tests

### Types de tests

- **Tests unitaires** : Testent des composants isolés
- **Tests d'intégration** : Testent l'interaction entre composants
- **Tests fonctionnels** : Testent les fonctionnalités complètes

### Exécution des tests

```bash
# Exécuter tous les tests
dotnet test

# Exécuter les tests d'un projet spécifique
dotnet test src/LogCentralPlatform.Tests/LogCentralPlatform.Tests.csproj
```

## Déploiement

### Création d'une release

1. Mettez à jour la version dans les fichiers .csproj
2. Créez une nouvelle branche `release/vX.Y.Z`
3. Assurez-vous que tous les tests passent
4. Fusionnez la branche release dans `main` et `develop`
5. Créez un tag pour la version

### Publication en production

1. Publiez l'API :
   ```bash
   dotnet publish src/LogCentralPlatform.Api -c Release -o ./publish/api
   ```

2. Publiez l'application Web :
   ```bash
   dotnet publish src/LogCentralPlatform.Web -c Release -o ./publish/web
   ```

3. Déployez les dossiers de publication sur votre serveur

## Ressources additionnelles

- [Documentation Microsoft ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Documentation Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Documentation n8n](https://docs.n8n.io/)

## Résolution de problèmes courants

### Base de données

**Problème** : Erreur "Cannot open database"
**Solution** : Vérifiez que le serveur SQL est en cours d'exécution et que la chaîne de connexion est correcte

**Problème** : Erreur de migration
**Solution** : Supprimez le dossier "Migrations" et recréez les migrations :
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Erreurs de compilation

**Problème** : Packages NuGet non restaurés
**Solution** : Exécutez `dotnet restore` à la racine du projet

**Problème** : Version .NET incorrecte
**Solution** : Vérifiez que vous utilisez .NET 8.0 avec `dotnet --version`

**Problème** : ViewModels manquants
**Solution** : Assurez-vous que tous les ViewModels référencés dans les vues sont correctement définis dans le dossier `src/LogCentralPlatform.Web/ViewModels/`

## Contacts

- **Responsable technique** : [nom@email.com](mailto:nom@email.com)
- **Responsable du projet** : [chef@email.com](mailto:chef@email.com)