using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Implementations
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public ProfileRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public Optional<IEnumerable<InferenceProfile>> GetProfiles()
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                var entities = repository.Fetch<InferenceProfile>();
                return entities.Count == 0 ? 
                    Optional<IEnumerable<InferenceProfile>>.Empty() :
                    Optional<IEnumerable<InferenceProfile>>.For(entities);
            }
        }

        public Optional<InferenceProfile> GetProfileByName(string profileName)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                var entity = repository.FirstOrDefault<InferenceProfile>(p => p.ProfileName == profileName);
                return entity == null ? Optional<InferenceProfile>.Empty() : Optional<InferenceProfile>.For(entity);
            }
        }

        public bool SaveProfile(InferenceProfile item)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                return repository.Upsert(item);
            }
        }

        public int DeleteProfile(string profileName)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                return repository.Delete<InferenceProfile>(p => p.ProfileName == profileName);
            }
        }
    }
}