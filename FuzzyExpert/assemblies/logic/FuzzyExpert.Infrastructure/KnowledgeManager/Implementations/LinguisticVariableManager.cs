using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class LinguisticVariableManager : ILinguisticVariableManager
    {
        private readonly ILinguisticVariableProvider _linguisticVariableProvider;

        public LinguisticVariableManager(ILinguisticVariableProvider linguisticVariableProvider)
        {
            _linguisticVariableProvider = linguisticVariableProvider ?? throw new ArgumentNullException(nameof(linguisticVariableProvider));
        }

        private Optional<Dictionary<int, LinguisticVariable>> _linguisticVariables = Optional<Dictionary<int, LinguisticVariable>>.Empty();

        public Optional<Dictionary<int, LinguisticVariable>> LinguisticVariables
        {
            get
            {
                if (_linguisticVariables.IsPresent) return _linguisticVariables;

                Optional<List<LinguisticVariable>> linguisticVariablesFromProvider = _linguisticVariableProvider.GetLinguisticVariables();
                if (!linguisticVariablesFromProvider.IsPresent) return Optional<Dictionary<int, LinguisticVariable>>.Empty();

                Dictionary<int, LinguisticVariable> linguisticVariables = new Dictionary<int, LinguisticVariable>();
                for (int i = 1; i <= linguisticVariablesFromProvider.Value.Count; i++)
                {
                    linguisticVariables.Add(i, linguisticVariablesFromProvider.Value[i - 1]);
                }
                return _linguisticVariables = Optional<Dictionary<int, LinguisticVariable>>.For(linguisticVariables);
            }
        }
    }
}