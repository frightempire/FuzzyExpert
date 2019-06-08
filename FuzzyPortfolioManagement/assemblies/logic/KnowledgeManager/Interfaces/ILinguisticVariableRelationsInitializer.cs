using System.Collections.Generic;
using KnowledgeManager.Entities;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableRelationsInitializer
    {
        List<LinguisticVariableRelations> FormRelations(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables);
    }
}