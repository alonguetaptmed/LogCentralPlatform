# Résolution des erreurs de build

Ce document contient des informations sur la résolution des erreurs de build courantes dans le projet LogCentralPlatform.

## Erreurs NETSDK1004 - Fichiers project.assets.json manquants

Si vous rencontrez des erreurs du type :

```
Erreur NETSDK1004 Le fichier de composants 'C:\...\obj\project.assets.json' est introuvable. Exécutez une restauration de package NuGet pour générer ce fichier.
```

### Solution

Ces erreurs indiquent que les packages NuGet n'ont pas été restaurés pour les projets de la solution. Pour résoudre ce problème :

#### Option 1 : Via Visual Studio

1. Clic droit sur la solution dans l'Explorateur de solutions
2. Sélectionnez "Restaurer les packages NuGet"

#### Option 2 : Via la ligne de commande

1. Ouvrez une console de commande ou un terminal à la racine de votre solution
2. Exécutez la commande suivante :
   ```
   dotnet restore
   ```

Après la restauration des packages, essayez de reconstruire la solution.

## Erreurs de référence à des namespaces

Si vous rencontrez des erreurs du type :

```
CS0103 Le nom 'Core' n'existe pas dans le contexte actuel
CS0246 Le nom de type ou d'espace de noms 'Core' est introuvable
```

### Solution

Ces erreurs indiquent généralement un problème avec les directives `using` ou des références incorrectes dans les fichiers de code. Assurez-vous que :

1. Les directives `using` appropriées sont présentes dans les fichiers concernés
2. Les références de projet sont correctement configurées
3. Les namespaces dans le code correspondent aux namespaces définis dans le projet

Si les erreurs persistent malgré les corrections, vous pouvez essayer de :

1. Nettoyer la solution (Clean Solution)
2. Supprimer les dossiers bin et obj
3. Restaurer les packages NuGet
4. Reconstruire la solution
