using LogCentralPlatform.Core.Entities;
using LogCentralPlatform.Core.Interfaces;
using LogCentralPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogCentralPlatform.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation du repository pour les logs.
    /// </summary>
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LogRepository> _logger;

        /// <summary>
        /// Constructeur du repository de logs.
        /// </summary>
        /// <param name="context">Contexte de base de données.</param>
        /// <param name="logger">Logger.</param>
        public LogRepository(ApplicationDbContext context, ILogger<LogRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<LogEntry> AddAsync(LogEntry logEntry)
        {
            try
            {
                await _context.LogEntries.AddAsync(logEntry);
                await _context.SaveChangesAsync();
                return logEntry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout d'un log");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<LogEntry?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.LogEntries.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du log {LogId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LogEntry>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 100)
        {
            try
            {
                return await _context.LogEntries
                    .Where(l => l.ServiceId == serviceId)
                    .OrderByDescending(l => l.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des logs pour le service {ServiceId}", serviceId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LogEntry>> GetByClientIdAsync(Guid clientId, int skip = 0, int take = 100)
        {
            try
            {
                return await _context.LogEntries
                    .Where(l => l.ClientId == clientId)
                    .OrderByDescending(l => l.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des logs pour le client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LogEntry>> GetByLevelAsync(LogLevel level, int skip = 0, int take = 100)
        {
            try
            {
                return await _context.LogEntries
                    .Where(l => l.Level == level)
                    .OrderByDescending(l => l.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des logs de niveau {Level}", level);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LogEntry>> SearchAsync(
            DateTime startDate, 
            DateTime endDate, 
            Guid? serviceId = null, 
            Guid? clientId = null, 
            LogLevel? level = null, 
            int skip = 0, 
            int take = 100)
        {
            try
            {
                var query = _context.LogEntries
                    .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

                if (serviceId.HasValue)
                {
                    query = query.Where(l => l.ServiceId == serviceId.Value);
                }

                if (clientId.HasValue)
                {
                    query = query.Where(l => l.ClientId == clientId.Value);
                }

                if (level.HasValue)
                {
                    query = query.Where(l => l.Level >= level.Value);
                }

                return await query
                    .OrderByDescending(l => l.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de logs");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LogEntry>> SearchByTextAsync(
            string searchText, 
            DateTime? startDate = null, 
            DateTime? endDate = null, 
            Guid? serviceId = null, 
            Guid? clientId = null, 
            int skip = 0, 
            int take = 100)
        {
            try
            {
                // Normalisation des paramètres de recherche
                searchText = searchText.ToLower();
                var start = startDate ?? DateTime.UtcNow.AddDays(-7);
                var end = endDate ?? DateTime.UtcNow;

                var query = _context.LogEntries
                    .Where(l => l.Timestamp >= start && l.Timestamp <= end)
                    .Where(l => l.Message.ToLower().Contains(searchText) || 
                                l.Category.ToLower().Contains(searchText) ||
                                (l.ExceptionDetails != null && l.ExceptionDetails.ToLower().Contains(searchText)) ||
                                (l.StackTrace != null && l.StackTrace.ToLower().Contains(searchText)));

                if (serviceId.HasValue)
                {
                    query = query.Where(l => l.ServiceId == serviceId.Value);
                }

                if (clientId.HasValue)
                {
                    query = query.Where(l => l.ClientId == clientId.Value);
                }

                return await query
                    .OrderByDescending(l => l.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de logs par texte");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAIAnalysisAsync(Guid id, string analysisResult)
        {
            try
            {
                var log = await _context.LogEntries.FindAsync(id);
                if (log == null)
                {
                    return false;
                }

                log.AnalyzedByAI = true;
                log.AIAnalysisResult = analysisResult;

                _context.LogEntries.Update(log);
                var updated = await _context.SaveChangesAsync();
                return updated > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de l'analyse IA pour le log {LogId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<int> GetCountByServiceIdAsync(Guid serviceId)
        {
            try
            {
                return await _context.LogEntries
                    .Where(l => l.ServiceId == serviceId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des logs pour le service {ServiceId}", serviceId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<int> GetCountByClientIdAsync(Guid clientId)
        {
            try
            {
                return await _context.LogEntries
                    .Where(l => l.ClientId == clientId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des logs pour le client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<int> GetCountByLevelAsync(LogLevel level)
        {
            try
            {
                return await _context.LogEntries
                    .Where(l => l.Level == level)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des logs de niveau {Level}", level);
                throw;
            }
        }
    }
}