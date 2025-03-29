using LogCentralPlatform.Core.Interfaces;
using LogCentralPlatform.Infrastructure.Data;
using LogCentralPlatform.Infrastructure.Repositories;
using LogCentralPlatform.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LogCentralPlatform.Infrastructure
{
    /// <summary>
    /// Configuration des dépendances pour la couche d'infrastructure.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Ajoute les services d'infrastructure au conteneur de dépendances.
        /// </summary>
        /// <param name="services">Collection de services.</param>
        /// <param name="configuration">Configuration de l'application.</param>
        /// <returns>La collection de services mise à jour.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration de la base de données
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Injection des repositories
            services.AddScoped<ILogRepository, LogRepository>();
            
            // Note: Les autres repositories seront implémentés ultérieurement
            // services.AddScoped<IServiceRepository, ServiceRepository>();
            // services.AddScoped<IClientRepository, ClientRepository>();

            // Injection des services
            services.AddScoped<IAIAnalysisService, AIAnalysisService>();
            // services.AddScoped<IAuthService, AuthService>();
            // services.AddScoped<INotificationService, NotificationService>();

            // Configuration du client HTTP pour n8n
            services.AddHttpClient("N8nClient", client =>
            {
                client.BaseAddress = new Uri(configuration["AISettings:N8nApiUrl"] ?? "http://localhost:5678");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}