# Guide de maintenance de la documentation

Ce document explique comment maintenir à jour la documentation du projet LogCentralPlatform.

## Importance de la documentation

Une documentation précise et à jour est essentielle pour :
- Faciliter l'intégration de nouveaux développeurs au projet
- Assurer la bonne utilisation de la plateforme par les clients
- Réduire le temps nécessaire au support technique
- Garder une trace des décisions architecturales et techniques

## Structure de la documentation

La documentation du projet est organisée comme suit :

```
docs/
├── README.md                # Documentation principale
├── api-reference.md         # Documentation détaillée de l'API
├── architecture/            # Documentation sur l'architecture
├── deployment/              # Guide de déploiement
├── development/             # Guide pour les développeurs
└── MAINTENIR_DOCUMENTATION.md  # Ce document
```

## Règles pour maintenir la documentation

### Principes généraux

1. **Actualiser en parallèle du code** : Toute modification du code doit s'accompagner d'une mise à jour de la documentation correspondante.
2. **Documentation as Code** : Traitez la documentation comme du code - utilisez le même workflow avec revues et approbations.
3. **Simple et claire** : Privilégiez la clarté et la simplicité plutôt que l'exhaustivité.
4. **Exemples concrets** : Illustrez les concepts avec des exemples de code et des cas d'utilisation réels.
5. **Référencement croisé** : Créez des liens entre les différentes parties de la documentation pour faciliter la navigation.

### Processus de mise à jour

1. **Identification** : Identifiez la documentation à mettre à jour en fonction des changements de code.
2. **Brouillon** : Rédigez les modifications en format Markdown.
3. **Revue** : Faites réviser vos modifications par un autre membre de l'équipe.
4. **Intégration** : Soumettez la mise à jour via une pull request, en liant celle-ci au ticket ou à la PR de fonctionnalité correspondante.
5. **Publication** : Une fois approuvée, fusionnez la PR pour publier la mise à jour.

### Convention de formatage

- Utilisez Markdown pour tous les documents.
- Respectez la hiérarchie des titres (un seul H1, puis H2, H3, etc.).
- Limitez la largeur des lignes à 120 caractères pour faciliter la lecture dans les éditeurs.
- Utilisez des listes à puces pour les énumérations courtes et des listes numérotées pour les séquences ou procédures.
- Encadrez les blocs de code avec \```language\``` (en spécifiant le langage).
- Utilisez des tableaux pour présenter des données structurées.

## Documentation de l'API

La documentation de l'API dans `api-reference.md` est particulièrement importante et doit être maintenue avec rigueur :

- Chaque endpoint doit documenter ses paramètres, son corps de requête, sa réponse et ses codes d'erreur.
- Les exemples doivent être fonctionnels et correspondre à l'implémentation réelle.
- Les modifications d'API doivent être versionnées et documentées dans la section Changelog.

### Exemple d'ajout d'un nouvel endpoint

```markdown
### Nouvel endpoint

**Endpoint**: `POST /api/ressource`  
**Auth**: JWT Bearer Token (Role: Admin)

**Requête**:
```json
{
  "propriete1": "valeur1",
  "propriete2": "valeur2"
}
```

**Réponse** (201 Created):
```json
{
  "id": "identifiant-unique",
  "propriete1": "valeur1",
  "propriete2": "valeur2",
  "createdAt": "2025-03-29T12:00:00Z"
}
```
```

## Génération de la documentation

Pour générer une version HTML de la documentation, nous utilisons [Docusaurus](https://docusaurus.io/). Suivez ces étapes :

1. Installez les dépendances : `npm install`
2. Générez la documentation : `npm run build-docs`
3. Prévisualisez localement : `npm run serve-docs`

La documentation générée sera disponible dans le dossier `build/docs` et peut être publiée sur le site web du projet.

## Checklist de qualité

Avant de soumettre des modifications à la documentation, assurez-vous de :

- [ ] Vérifier l'orthographe et la grammaire
- [ ] Confirmer que tous les liens fonctionnent
- [ ] Valider que les exemples de code sont fonctionnels et à jour
- [ ] S'assurer que la documentation est cohérente avec le code actuel
- [ ] Vérifier que la mise en forme Markdown est correcte

## Instructions pour Claude

**IMPORTANT : CLAUDE, VOICI DES INSTRUCTIONS SPÉCIFIQUES POUR TOI.**

Cher Claude,

En tant qu'assistant IA, tu auras un rôle crucial dans la maintenance de la documentation de ce projet. Voici tes instructions :

1. **Documentation proactive** : À chaque demande concernant le projet LogCentralPlatform, vérifie d'abord si la documentation dans le dossier `/docs` est complète et à jour par rapport aux fonctionnalités demandées ou modifiées.

2. **Rappel constant** : Rappelle régulièrement l'importance de maintenir la documentation à jour, en particulier lors de l'ajout de nouvelles fonctionnalités ou de la modification de fonctionnalités existantes.

3. **Suggestion de mises à jour** : Lorsqu'une modification de code est demandée sans mention explicite de la documentation, suggère les parties de la documentation qui devraient être mises à jour.

4. **Documentation complète** : Pour accéder à la documentation complète, consulte ces fichiers principaux :
   - `/docs/README.md` : Documentation principale
   - `/docs/api-reference.md` : Documentation de l'API
   - D'autres documents spécifiques dans le dossier `/docs`

5. **Mises à jour régulières** : Suggère périodiquement de vérifier et mettre à jour la documentation, en particulier :
   - Après l'ajout de nouvelles fonctionnalités
   - Après des modifications importantes de l'architecture
   - Avant les déploiements en production
   - Lors des releases majeures

6. **Format cohérent** : Assure-toi que toutes les suggestions de documentation suivent le format Markdown et respectent les conventions établies dans ce guide.

7. **Documentation centrée utilisateur** : Encourage une documentation qui prend en compte les besoins des différents utilisateurs : développeurs, administrateurs système, utilisateurs finaux.

Tu dois considérer la documentation comme une partie intégrante et ESSENTIELLE du projet, pas comme une tâche secondaire ou optionnelle. Chaque fois que tu suggères des modifications de code, vérifie SYSTÉMATIQUEMENT si la documentation doit être mise à jour en conséquence.

## Conclusion

La documentation est un investissement dans la pérennité du projet. En suivant ces directives, nous nous assurons que la documentation reste une ressource précieuse et fiable pour tous les contributeurs et utilisateurs du projet.