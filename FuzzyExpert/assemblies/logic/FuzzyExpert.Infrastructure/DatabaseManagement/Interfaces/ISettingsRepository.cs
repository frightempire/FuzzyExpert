using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces
{
    public interface ISettingsRepository
    {
        Optional<Settings> GetSettingsForUser(string userName);

        int GetMaxSettingsId();

        bool SaveSettings(Settings item);
    }
}