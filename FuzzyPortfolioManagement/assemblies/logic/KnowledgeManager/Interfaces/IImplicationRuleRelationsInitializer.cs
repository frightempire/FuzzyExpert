using System.Collections.Generic;
using KnowledgeManager.Entities;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IImplicationRuleRelationsInitializer
    {
        List<ImplicationRuleRelations> FormImplicationRuleRelations(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables);
    }
}