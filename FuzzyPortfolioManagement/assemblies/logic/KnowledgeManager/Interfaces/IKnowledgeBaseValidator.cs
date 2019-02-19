using System.Collections.Generic;
using CommonLogic.Entities;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IKnowledgeBaseValidator
    {
        ValidationOperationResult ValidateLinguisticVariablesNames(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables);
    }
}
