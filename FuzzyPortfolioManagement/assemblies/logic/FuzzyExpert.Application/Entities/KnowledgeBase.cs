using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Application.Entities
{
    public class KnowledgeBase
    {
        public KnowledgeBase(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables,
            List<LinguisticVariableRelations> linguisticVariablesRelations)
        {
            if (implicationRules == null) throw new ArgumentNullException(nameof(implicationRules));
            if (linguisticVariables == null) throw new ArgumentNullException(nameof(linguisticVariables));
            if (linguisticVariablesRelations == null) throw new ArgumentNullException(nameof(linguisticVariablesRelations));
            if (!implicationRules.Any()) throw new ArgumentNullException(nameof(linguisticVariablesRelations));
            if (!linguisticVariables.Any()) throw new ArgumentNullException(nameof(linguisticVariablesRelations));
            if (!linguisticVariablesRelations.Any()) throw new ArgumentNullException(nameof(linguisticVariablesRelations));

            ImplicationRules = implicationRules;
            LinguisticVariables = linguisticVariables;
            LinguisticVariablesRelations = linguisticVariablesRelations;
        }

        public Dictionary<int, ImplicationRule> ImplicationRules { get; }

        public Dictionary<int, LinguisticVariable> LinguisticVariables { get; }

        public List<LinguisticVariableRelations> LinguisticVariablesRelations { get; }
    }
}