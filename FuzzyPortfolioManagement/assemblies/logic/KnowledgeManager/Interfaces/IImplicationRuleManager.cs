using System.Collections.Generic;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IImplicationRuleManager
    {
        List<ImplicationRule> ImplicationRules { get; }
    }
}