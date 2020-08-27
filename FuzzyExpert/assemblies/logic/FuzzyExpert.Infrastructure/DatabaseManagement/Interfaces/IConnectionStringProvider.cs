using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces
{
    public interface IConnectionStringProvider
    {
        ConnectionString ConnectionString { get; }
    }
}