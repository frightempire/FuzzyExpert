using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces
{
    public interface IDefaultSettingsProvider
    {
        DefaultSettings Settings { get; }
    }
}