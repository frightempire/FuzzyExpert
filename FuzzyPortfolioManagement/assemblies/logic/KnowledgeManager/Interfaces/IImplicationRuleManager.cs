using System.Collections.Generic;
using CommonLogic.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IImplicationRuleManager
    {
        Optional<Dictionary<int, ImplicationRule>> ImplicationRules { get; }
    }
}