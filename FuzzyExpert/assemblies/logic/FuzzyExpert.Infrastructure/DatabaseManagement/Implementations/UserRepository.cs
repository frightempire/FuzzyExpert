using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public Optional<IEnumerable<User>> GetUsers()
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                var entities = repository.Fetch<User>();
                return entities.Count == 0 ?
                    Optional<IEnumerable<User>>.Empty() :
                    Optional<IEnumerable<User>>.For(entities);
            }
        }

        public Optional<User> GetUserByName(string userName)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                var entity = repository.FirstOrDefault<User>(p => p.UserName == userName);
                return entity == null ? Optional<User>.Empty() : Optional<User>.For(entity);
            }
        }

        public bool SaveUser(User item)
        {
            using (var repository = new LiteRepository(_connectionStringProvider.ConnectionString))
            {
                return repository.Upsert(item);
            }
        }
    }
}