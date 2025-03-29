# LogCentralPlatform

Plateforme centralisée pour la gestion, l'analyse et la surveillance des logs d'applications en ASP.NET Core MVC.

## Objectif du projet

L'objectif est de développer une plateforme web en ASP.NET C# MVC permettant de centraliser, analyser et surveiller les logs des applications et services Windows déployés chez nos clients.

Actuellement, ces logs sont stockés localement et les erreurs sont parfois remontées par e-mail, ce qui complique leur suivi et leur exploitation.

## Fonctionnalités principales

### 1. Collecte et stockage des logs via API

- Développement d'une API REST sécurisée permettant aux applications déployées de pousser leurs logs en temps réel.
- Attribution d'un identifiant unique (clé API) à chaque service déployé pour assurer une traçabilité et une classification des logs.
- Stockage des logs en base de données avec une architecture optimisée pour la recherche et l'analyse (ex : SQL Server + indexation).

### 2. Suivi et gestion des services

- Interface utilisateur moderne et intuitive permettant de visualiser l'état des services en temps réel.
- Possibilité de définir un intervalle minimal de reporting pour chaque service.
- Alertes visuelles et notifications en cas :
  - D'erreur critique détectée dans les logs.
  - D'absence de communication d'un service au-delà de son intervalle défini.
  - D'événements spécifiques prédéfinis.
- Gestion des interfaces spécifiques liées aux clients avec rattachement des développements associés.

### 3. Débogage et exploitation par une IA (n8n)

- Intégration d'un agent IA (n8n) capable d'analyser les logs et de détecter des anomalies récurrentes.
- Possibilité pour l'IA d'accéder au code source des applications mises à disposition afin d'aider au diagnostic et à la correction des erreurs.
- Recommandations automatiques pour le débogage des applications déployées.
- Suggestion d'optimisations ou de bonnes pratiques en fonction des erreurs détectées.

### 4. Sécurité et gestion des accès

- Authentification et gestion des droits utilisateurs (admin, support, client...).
- Chiffrement des logs sensibles pour garantir la confidentialité.
- Journalisation des accès et des actions pour assurer une traçabilité complète.

## Technologies et outils prévus

- Back-end : ASP.NET Core MVC, Entity Framework, SQL Server
- Front-end : Bootstrap, Vue.js/React pour une interface dynamique
- API & Sécurité : JWT pour l'authentification, API REST
- Monitoring & IA : n8n pour l'analyse des logs et le support au débogage

## Installation et démarrage

*À venir*

## Structure du projet

*À venir*

## Contribuer

*À venir*

## Licence

*À définir*