using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces
{
    public interface IUserRepository
    {
        Optional<IEnumerable<User>> GetUsers();

        Optional<User> GetUserByName(string userName);

        bool SaveUser(User item);
    }
}