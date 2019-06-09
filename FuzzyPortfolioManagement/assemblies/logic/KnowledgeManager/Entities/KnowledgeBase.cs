using System.Collections.Generic;
using CommonLogic;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Entities
{
    public class KnowledgeBase
    {
        public KnowledgeBase(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables,
            List<LinguisticVariableRelations> linguisticVariablesRelations)
        {
            ExceptionAssert.IsNull(implicationRules);
            ExceptionAssert.IsNull(linguisticVariables);
            ExceptionAssert.IsNull(linguisticVariablesRelations);
            ExceptionAssert.IsEmpty(implicationRules);
            ExceptionAssert.IsEmpty(linguisticVariables);
            ExceptionAssert.IsEmpty(linguisticVariablesRelations);

            ImplicationRules = implicationRules;
            LinguisticVariables = linguisticVariables;
            LinguisticVariablesRelations = linguisticVariablesRelations;
        }

        public Dictionary<int, ImplicationRule> ImplicationRules { get; }

        public Dictionary<int, LinguisticVariable> LinguisticVariables { get; }

        public List<LinguisticVariableRelations> LinguisticVariablesRelations { get; }
    }
}