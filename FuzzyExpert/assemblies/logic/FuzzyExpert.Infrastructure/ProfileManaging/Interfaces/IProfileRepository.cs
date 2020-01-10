using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;

namespace FuzzyExpert.Infrastructure.ProfileManaging.Interfaces
{
    public interface IProfileRepository
    {
        Optional<IEnumerable<InferenceProfile>> GetProfiles();

        Optional<InferenceProfile> GetProfileByName(string profileName);

        bool SaveProfile(InferenceProfile item);

        int DeleteProfile(string profileName);
    }
}