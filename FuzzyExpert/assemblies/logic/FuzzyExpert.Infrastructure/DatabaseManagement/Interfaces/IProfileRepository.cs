using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces
{
    public interface IProfileRepository
    {
        Optional<IEnumerable<InferenceProfile>> GetProfilesForUser(string userName);

        Optional<InferenceProfile> GetProfileByName(string profileName);

        bool SaveProfile(InferenceProfile item);

        int DeleteProfile(string profileName);
    }
}