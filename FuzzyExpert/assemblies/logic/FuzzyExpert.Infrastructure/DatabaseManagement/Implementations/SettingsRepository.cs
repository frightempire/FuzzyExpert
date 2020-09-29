using System;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Implementations
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SettingsRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public Optional<Settings> GetSettingsForUser(string userName)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                var entity = repository.FirstOrDefault<Settings>(p => p.UserName == userName);
                return entity == null ? Optional<Settings>.Empty() : Optional<Settings>.For(entity);
            }
        }

        public int GetMaxSettingsId()
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                var entities = repository.Query<Settings>().ToList();
                return entities == null || entities.Count == 0 ? 0 : entities.Max(e => e.Id); 
            }
        }

        public bool SaveSettings(Settings item)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                return repository.Upsert(item);
            }
        }
    }
}