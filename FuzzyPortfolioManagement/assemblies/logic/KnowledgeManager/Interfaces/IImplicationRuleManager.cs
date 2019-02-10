using System.Collections.Generic;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IImplicationRuleManager
    {
        Dictionary<int, ImplicationRule> ImplicationRules { get; }
    }
}