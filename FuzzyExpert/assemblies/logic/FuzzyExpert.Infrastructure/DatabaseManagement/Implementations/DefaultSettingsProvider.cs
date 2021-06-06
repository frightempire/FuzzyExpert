using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces
{
    public class DefaultSettingsProvider : IDefaultSettingsProvider
    {
        public DefaultSettings Settings => new DefaultSettings(0.8);
    }
}