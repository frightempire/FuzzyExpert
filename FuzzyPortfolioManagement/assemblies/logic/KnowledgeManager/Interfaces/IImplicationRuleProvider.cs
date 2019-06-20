using System.Collections.Generic;
using CommonLogic.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IImplicationRuleProvider
    {
        Optional<List<ImplicationRule>> GetImplicationRules();
    }
}
