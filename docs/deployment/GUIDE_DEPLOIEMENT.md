# Guide de déploiement

Ce document détaille les étapes nécessaires pour déployer LogCentralPlatform dans un environnement de production.

## Prérequis

### Matériel recommandé

- **Serveur d'application** :
  - CPU : 4 cœurs minimum
  - RAM : 8 Go minimum
  - Disque : 100 Go minimum (SSD recommandé)

- **Serveur de base de données** :
  - CPU : 4 cœurs minimum
  - RAM : 16 Go minimum
  - Disque : 500 Go minimum (SSD recommandé)

### Logiciels requis

- **Serveur d'application** :
  - Windows Server 2019 ou plus récent / Linux (Ubuntu 20.04 LTS ou plus récent)
  - .NET 8.0 Runtime
  - IIS (Windows) ou Nginx/Apache (Linux)
  - n8n (optionnel, pour l'analyse IA)

- **Serveur de base de données** :
  - SQL Server 2019 ou plus récent

## Déploiement de la base de données

### Installation de SQL Server

1. Installez SQL Server en suivant la [documentation officielle](https://docs.microsoft.com/fr-fr/sql/database-engine/install-windows/install-sql-server)

2. Configurez SQL Server pour autoriser les connexions à distance :
   - Ouvrez SQL Server Configuration Manager
   - Activez TCP/IP dans les protocoles du serveur
   - Redémarrez le service SQL Server

3. Créez un utilisateur dédié pour l'application :
   ```sql
   CREATE LOGIN LogCentralApp WITH PASSWORD = 'VotreMotDePasseComplexe';
   CREATE USER LogCentralApp FOR LOGIN LogCentralApp;
   ```

### Création de la base de données

1. Créez la base de données :
   ```sql
   CREATE DATABASE LogCentralPlatform;
   GO
   USE LogCentralPlatform;
   GO
   ```

2. Accordez les permissions à l'utilisateur :
   ```sql
   USE LogCentralPlatform;
   GO
   EXEC sp_addrolemember 'db_owner', 'LogCentralApp';
   GO
   ```

3. Appliquez les migrations depuis le serveur d'application :
   ```bash
   dotnet ef database update --connection "Server=<serveur-sql>;Database=LogCentralPlatform;User Id=LogCentralApp;Password=VotreMotDePasseComplexe;TrustServerCertificate=True"
   ```

## Déploiement de l'application

### Préparation des fichiers

1. Sur votre environnement de développement, publiez les projets :
   ```bash
   # API
   dotnet publish src/LogCentralPlatform.Api -c Release -o ./publish/api

   # Web
   dotnet publish src/LogCentralPlatform.Web -c Release -o ./publish/web
   ```

2. Configurez les fichiers appsettings.json pour l'environnement de production :
   - Mettez à jour les chaînes de connexion
   - Modifiez les paramètres JWT avec des clés sécurisées
   - Configurez les URLs et paramètres CORS
   - Ajustez les paramètres de logging

### Déploiement sous Windows (IIS)

1. Installez le [.NET 8.0 Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/8.0)

2. Créez deux sites IIS :
   - **API** : 
     - Nom : LogCentralPlatform-API
     - Chemin physique : C:\inetpub\wwwroot\LogCentralPlatform-API
     - Port : 443 (avec certificat SSL)
   
   - **Web** : 
     - Nom : LogCentralPlatform-Web
     - Chemin physique : C:\inetpub\wwwroot\LogCentralPlatform-Web
     - Port : 443 (avec certificat SSL)

3. Configurez les pools d'applications :
   - Framework : No Managed Code
   - Mode pipeline : Integrated
   - Identity : ApplicationPoolIdentity ou compte dédié

4. Copiez les fichiers publiés dans les dossiers respectifs

5. Configurez les permissions des dossiers pour le pool d'applications

6. Configurez les certificats SSL dans IIS Manager

### Déploiement sous Linux (Nginx)

1. Installez le runtime .NET 8.0 :
   ```bash
   wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y aspnetcore-runtime-8.0
   ```

2. Créez les dossiers pour l'application :
   ```bash
   sudo mkdir -p /var/www/logcentralplatform/api
   sudo mkdir -p /var/www/logcentralplatform/web
   ```

3. Copiez les fichiers publiés dans les dossiers respectifs

4. Créez les services systemd :
   - Créez `/etc/systemd/system/logcentralplatform-api.service` :
     ```
     [Unit]
     Description=LogCentralPlatform API

     [Service]
     WorkingDirectory=/var/www/logcentralplatform/api
     ExecStart=/usr/bin/dotnet /var/www/logcentralplatform/api/LogCentralPlatform.Api.dll
     Restart=always
     RestartSec=10
     KillSignal=SIGINT
     SyslogIdentifier=logcentralplatform-api
     User=www-data
     Environment=ASPNETCORE_ENVIRONMENT=Production
     Environment=ASPNETCORE_URLS=http://localhost:5000

     [Install]
     WantedBy=multi-user.target
     ```

   - Créez `/etc/systemd/system/logcentralplatform-web.service` :
     ```
     [Unit]
     Description=LogCentralPlatform Web

     [Service]
     WorkingDirectory=/var/www/logcentralplatform/web
     ExecStart=/usr/bin/dotnet /var/www/logcentralplatform/web/LogCentralPlatform.Web.dll
     Restart=always
     RestartSec=10
     KillSignal=SIGINT
     SyslogIdentifier=logcentralplatform-web
     User=www-data
     Environment=ASPNETCORE_ENVIRONMENT=Production
     Environment=ASPNETCORE_URLS=http://localhost:5002

     [Install]
     WantedBy=multi-user.target
     ```

5. Activez et démarrez les services :
   ```bash
   sudo systemctl enable logcentralplatform-api.service
   sudo systemctl start logcentralplatform-api.service
   sudo systemctl enable logcentralplatform-web.service
   sudo systemctl start logcentralplatform-web.service
   ```

6. Installez et configurez Nginx comme proxy inverse :
   ```bash
   sudo apt-get install -y nginx
   ```

7. Créez les configurations Nginx :
   - Créez `/etc/nginx/sites-available/logcentralplatform-api` :
     ```
     server {
         listen 443 ssl;
         server_name api.logcentralplatform.com;

         ssl_certificate /etc/ssl/certs/api.logcentralplatform.com.crt;
         ssl_certificate_key /etc/ssl/private/api.logcentralplatform.com.key;

         location / {
             proxy_pass http://localhost:5000;
             proxy_http_version 1.1;
             proxy_set_header Upgrade $http_upgrade;
             proxy_set_header Connection keep-alive;
             proxy_set_header Host $host;
             proxy_cache_bypass $http_upgrade;
             proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
             proxy_set_header X-Forwarded-Proto $scheme;
         }
     }
     ```

   - Créez `/etc/nginx/sites-available/logcentralplatform-web` :
     ```
     server {
         listen 443 ssl;
         server_name logcentralplatform.com;

         ssl_certificate /etc/ssl/certs/logcentralplatform.com.crt;
         ssl_certificate_key /etc/ssl/private/logcentralplatform.com.key;

         location / {
             proxy_pass http://localhost:5002;
             proxy_http_version 1.1;
             proxy_set_header Upgrade $http_upgrade;
             proxy_set_header Connection keep-alive;
             proxy_set_header Host $host;
             proxy_cache_bypass $http_upgrade;
             proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
             proxy_set_header X-Forwarded-Proto $scheme;
         }
     }
     ```

8. Activez les configurations et redémarrez Nginx :
   ```bash
   sudo ln -s /etc/nginx/sites-available/logcentralplatform-api /etc/nginx/sites-enabled/
   sudo ln -s /etc/nginx/sites-available/logcentralplatform-web /etc/nginx/sites-enabled/
   sudo nginx -t
   sudo systemctl restart nginx
   ```

## Configuration de n8n (optionnel)

Si vous souhaitez utiliser n8n pour l'analyse IA :

1. Installez n8n :
   ```bash
   npm install n8n -g
   ```

2. Créez un service systemd pour n8n :
   ```
   [Unit]
   Description=n8n
   After=network.target

   [Service]
   Type=simple
   User=n8n
   WorkingDirectory=/home/n8n
   ExecStart=/usr/bin/n8n start
   Restart=always
   RestartSec=10

   [Install]
   WantedBy=multi-user.target
   ```

3. Activez et démarrez le service :
   ```bash
   sudo systemctl enable n8n.service
   sudo systemctl start n8n.service
   ```

4. Configurez n8n derrière un proxy Nginx (recommandé pour la production)

5. Mettez à jour la configuration de l'API dans `appsettings.json` pour pointer vers votre instance n8n :
   ```json
   "AISettings": {
     "N8nApiUrl": "https://n8n.votredomaine.com/webhook",
     "N8nApiKey": "votre_clé_api_n8n",
     "AnalysisCronSchedule": "0 */4 * * *",
     "MaxConcurrentAnalyses": 5
   }
   ```

## Sécurité

### Renforcement de la sécurité

1. Activez le pare-feu (Windows Firewall ou ufw sur Linux) et n'ouvrez que les ports nécessaires (80, 443)

2. Mettez en place des certificats SSL valides pour tous les points d'entrée (Let's Encrypt ou certificat commercial)

3. Configurez des politiques de mot de passe fortes pour les comptes de base de données et d'application

4. Activez l'audit et la journalisation des accès et des actions

5. Mettez en place des sauvegardes régulières (voir section suivante)

### Configuration HTTPS

1. Obtenez des certificats SSL pour vos domaines
2. Configurez HTTPS dans IIS ou Nginx
3. Mettez en place HSTS pour forcer les connexions HTTPS
4. Configurez les en-têtes de sécurité appropriés

## Sauvegardes

### Sauvegarde de la base de données

1. Configurez des sauvegardes complètes quotidiennes :
   ```sql
   BACKUP DATABASE LogCentralPlatform TO DISK = 'C:\Backups\LogCentralPlatform_Full_$(date).bak' WITH INIT;
   ```

2. Configurez des sauvegardes différentielles toutes les 6 heures :
   ```sql
   BACKUP DATABASE LogCentralPlatform TO DISK = 'C:\Backups\LogCentralPlatform_Diff_$(date).bak' WITH DIFFERENTIAL;
   ```

3. Configurez des sauvegardes de journal toutes les heures :
   ```sql
   BACKUP LOG LogCentralPlatform TO DISK = 'C:\Backups\LogCentralPlatform_Log_$(date).trn';
   ```

4. Définissez une stratégie de rétention (par exemple, conserver les sauvegardes complètes pendant 30 jours)

### Sauvegarde des fichiers d'application

1. Sauvegardez régulièrement les fichiers de configuration
2. Incluez les fichiers de logs et de données générées
3. Stockez les sauvegardes dans un emplacement sécurisé, idéalement hors site

## Mise à jour

### Procédure de mise à jour

1. Sauvegardez la base de données et les fichiers de configuration

2. Arrêtez les services :
   ```bash
   # Windows
   Stop-Website "LogCentralPlatform-API"
   Stop-Website "LogCentralPlatform-Web"
   
   # Linux
   sudo systemctl stop logcentralplatform-api.service
   sudo systemctl stop logcentralplatform-web.service
   ```

3. Appliquez les migrations de base de données :
   ```bash
   dotnet ef database update --connection "..."
   ```

4. Déployez les nouveaux fichiers d'application

5. Redémarrez les services :
   ```bash
   # Windows
   Start-Website "LogCentralPlatform-API"
   Start-Website "LogCentralPlatform-Web"
   
   # Linux
   sudo systemctl start logcentralplatform-api.service
   sudo systemctl start logcentralplatform-web.service
   ```

6. Vérifiez que l'application fonctionne correctement

### Plan de rollback

En cas d'échec de la mise à jour :

1. Restaurez la base de données depuis la sauvegarde
2. Redéployez l'ancienne version des fichiers d'application
3. Redémarrez les services
4. Vérifiez que l'application fonctionne correctement

## Surveillance et maintenance

### Surveillance

1. Configurez des alertes pour :
   - Utilisation élevée du CPU/mémoire
   - Espace disque insuffisant
   - Indisponibilité du service
   - Erreurs dans les logs

2. Utilisez des outils de surveillance comme :
   - Application Insights (Azure)
   - Prometheus + Grafana
   - Zabbix
   - Nagios

3. Configurez des tests de disponibilité pour vérifier régulièrement que l'API et l'interface Web sont accessibles

### Journalisation

1. Collectez et centralisez les logs de l'application
2. Mettez en place une rotation des logs pour éviter de remplir le disque
3. Définissez une stratégie de rétention des logs (par exemple, conserver les logs pendant 90 jours)

### Maintenance régulière

1. Planifiez des fenêtres de maintenance mensuelles pour :
   - Appliquer les mises à jour de sécurité
   - Nettoyer les données inutiles
   - Optimiser la base de données

2. Effectuez des sauvegardes régulières et testez leur restauration

3. Vérifiez régulièrement l'utilisation des ressources et planifiez les capacités futures

## Résolution des problèmes courants

### Problèmes de base de données

- **Erreur de connexion** : Vérifiez que SQL Server est en cours d'exécution et que la chaîne de connexion est correcte
- **Lenteur des requêtes** : Analysez les index et optimisez les requêtes problématiques
- **Saturation de l'espace disque** : Nettoyez les logs inutiles ou augmentez l'espace disque

### Problèmes d'application

- **Erreur 500** : Consultez les logs d'application pour identifier la cause
- **Erreur 503** : Vérifiez que les services sont en cours d'exécution
- **Lenteur de l'application** : Vérifiez l'utilisation CPU/mémoire et optimisez si nécessaire

### Problèmes de réseau

- **Timeout** : Vérifiez les pare-feu et les règles de routage
- **Certificat SSL expiré** : Renouvelez le certificat SSL
- **Problèmes DNS** : Vérifiez la configuration DNS

## Contacts et support

- **Support technique** : support@logcentralplatform.com
- **Urgence** : +33 1 23 45 67 89

## Annexes

### Liste de vérification de déploiement

- [ ] Base de données créée et sécurisée
- [ ] Migrations appliquées
- [ ] Application déployée
- [ ] Certificats SSL installés
- [ ] Pare-feu configuré
- [ ] Sauvegardes configurées
- [ ] Surveillance mise en place
- [ ] Documentation mise à jour

### Références

- [Documentation ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Documentation SQL Server](https://docs.microsoft.com/en-us/sql/sql-server/)
- [Documentation IIS](https://docs.microsoft.com/en-us/iis/get-started/whats-new-in-iis-10-version-1709/new-features-introduced-in-iis-10-1709)
- [Documentation Nginx](https://nginx.org/en/docs/)
- [Documentation n8n](https://docs.n8n.io/)