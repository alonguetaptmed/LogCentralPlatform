using LogCentralPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LogCentralPlatform.Infrastructure.Data
{
    /// <summary>
    /// Contexte de base de données principal de l'application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Entrées de logs.
        /// </summary>
        public DbSet<LogEntry> LogEntries { get; set; }

        /// <summary>
        /// Services enregistrés.
        /// </summary>
        public DbSet<RegisteredService> Services { get; set; }

        /// <summary>
        /// Clients.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Utilisateurs.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Rôles des utilisateurs.
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Accès des utilisateurs aux clients.
        /// </summary>
        public DbSet<UserClientAccess> UserClientAccess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de l'entité LogEntry
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.ToTable("LogEntries");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ServiceVersion).HasMaxLength(20);
                entity.Property(e => e.Environment).HasMaxLength(50);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.ClientName).HasMaxLength(100);
                entity.Property(e => e.ExceptionDetails).HasMaxLength(4000);
                entity.Property(e => e.StackTrace).HasMaxLength(8000);
                entity.Property(e => e.CorrelationId).HasMaxLength(100);
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.AIAnalysisResult).HasMaxLength(4000);

                // Conversion du dictionnaire de métadonnées en JSON
                entity.Property(e => e.Metadata)
                    .HasConversion(
                        v => v != null ? JsonConvert.SerializeObject(v) : null,
                        v => v != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(v) : null);

                // Index pour améliorer les performances des requêtes
                entity.HasIndex(e => e.ServiceId);
                entity.HasIndex(e => e.ClientId);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.Level);
                entity.HasIndex(e => e.CorrelationId);
                entity.HasIndex(e => new { e.ServiceId, e.Timestamp });
                entity.HasIndex(e => new { e.ClientId, e.Timestamp });
            });

            // Configuration de l'entité RegisteredService
            modelBuilder.Entity<RegisteredService>(entity =>
            {
                entity.ToTable("Services");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Version).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ServiceType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ApiKey).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Environment).IsRequired().HasMaxLength(50);
                entity.Property(e => e.WebhookUrl).HasMaxLength(500);
                entity.Property(e => e.SourceCodePath).HasMaxLength(500);

                // Conversion des listes et dictionnaires en JSON
                entity.Property(e => e.AlertEmailRecipients)
                    .HasConversion(
                        v => v != null ? JsonConvert.SerializeObject(v) : null,
                        v => v != null ? JsonConvert.DeserializeObject<List<string>>(v) : new List<string>());

                entity.Property(e => e.Metadata)
                    .HasConversion(
                        v => v != null ? JsonConvert.SerializeObject(v) : null,
                        v => v != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(v) : null);

                // Index pour améliorer les performances des requêtes
                entity.HasIndex(e => e.ApiKey).IsUnique();
                entity.HasIndex(e => e.ClientId);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.Environment);
            });

            // Configuration de l'entité Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ClientNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(200);

                // Conversion des propriétés complexes en JSON
                entity.Property(e => e.Contacts)
                    .HasConversion(
                        v => v != null ? JsonConvert.SerializeObject(v) : null,
                        v => v != null ? JsonConvert.DeserializeObject<List<ContactPerson>>(v) : new List<ContactPerson>());

                entity.Property(e => e.NotificationSettings)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<NotificationSettings>(v) ?? new NotificationSettings());

                entity.Property(e => e.Metadata)
                    .HasConversion(
                        v => v != null ? JsonConvert.SerializeObject(v) : null,
                        v => v != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(v) : null);

                // Index pour améliorer les performances des requêtes
                entity.HasIndex(e => e.ClientNumber).IsUnique();
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.IsActive);
            });

            // Configuration de l'entité User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.EmailConfirmationToken).HasMaxLength(100);
                entity.Property(e => e.PasswordResetToken).HasMaxLength(100);
                entity.Property(e => e.TwoFactorSecret).HasMaxLength(50);

                // Conversion des propriétés complexes en JSON
                entity.Property(e => e.Preferences)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<UserPreferences>(v) ?? new UserPreferences());

                // Index pour améliorer les performances des requêtes
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.IsActive);
            });

            // Configuration de l'entité UserRole
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);

                // Index pour améliorer les performances des requêtes
                entity.HasIndex(e => new { e.UserId, e.RoleName }).IsUnique();
            });

            // Configuration de l'entité UserClientAccess
            modelBuilder.Entity<UserClientAccess>(entity =>
            {
                entity.ToTable("UserClientAccess");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);

                // Index pour améliorer les performances des requêtes
                entity.HasIndex(e => new { e.UserId, e.ClientId }).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.ClientId);
            });
        }
    }
}