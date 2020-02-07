using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class DatabaseLinguisticVariableProvider : ILinguisticVariableProvider
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ILinguisticVariableCreator _linguisticVariableCreator;

        public DatabaseLinguisticVariableProvider(
            IProfileRepository profileRepository,
            ILinguisticVariableCreator linguisticVariableCreator)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _linguisticVariableCreator = linguisticVariableCreator ?? throw new ArgumentNullException(nameof(linguisticVariableCreator));
        }

        public Optional<List<LinguisticVariable>> GetLinguisticVariables(string profileName)
        {
            var profile = _profileRepository.GetProfileByName(profileName);
            if (!profile.IsPresent || profile.Value.Variables == null || !profile.Value.Variables.Any())
            {
                return Optional<List<LinguisticVariable>>.Empty();
            }

            List<LinguisticVariable> linguisticVariables = new List<LinguisticVariable>();
            foreach (var variable in profile.Value.Variables)
            {
                LinguisticVariable linguisticVariable = _linguisticVariableCreator.CreateLinguisticVariableEntity(variable);
                linguisticVariables.Add(linguisticVariable);
            }

            return Optional<List<LinguisticVariable>>.For(linguisticVariables);
        }
    }
}