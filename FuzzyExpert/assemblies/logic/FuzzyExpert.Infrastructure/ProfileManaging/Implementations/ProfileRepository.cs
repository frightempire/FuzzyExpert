using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using LiteDB;

namespace FuzzyExpert.Infrastructure.ProfileManaging.Implementations
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ConnectionString _connectionString = new ConnectionString
        {
            Filename = "Inference.db",
            Mode = FileMode.Exclusive
        };

        public Optional<IEnumerable<InferenceProfile>> GetProfiles()
        {
            using (var repository = new LiteRepository(_connectionString))
            {
                var entities = repository.Fetch<InferenceProfile>();
                return entities.Count == 0 ? 
                    Optional<IEnumerable<InferenceProfile>>.Empty() :
                    Optional<IEnumerable<InferenceProfile>>.For(entities);
            }
        }

        public Optional<InferenceProfile> GetProfileByName(string profileName)
        {
            using (var repository = new LiteRepository(_connectionString))
            {
                var entity = repository.FirstOrDefault<InferenceProfile>(p => p.ProfileName == profileName);
                return entity == null ? Optional<InferenceProfile>.Empty() : Optional<InferenceProfile>.For(entity);
            }
        }

        public bool SaveProfile(InferenceProfile item)
        {
            using (var repository = new LiteRepository(_connectionString))
            {
                return repository.Upsert(item);
            }
        }

        public int DeleteProfile(string profileName)
        {
            using (var repository = new LiteRepository(_connectionString))
            {
                return repository.Delete<InferenceProfile>(p => p.ProfileName == profileName);
            }
        }
    }
}