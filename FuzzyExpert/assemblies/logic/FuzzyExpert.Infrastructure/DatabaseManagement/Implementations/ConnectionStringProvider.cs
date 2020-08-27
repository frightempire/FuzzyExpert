using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Implementations
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public ConnectionString ConnectionString =>
            new ConnectionString
            {
                Filename = "Inference.db",
                Mode = FileMode.Exclusive
            };
    }
}