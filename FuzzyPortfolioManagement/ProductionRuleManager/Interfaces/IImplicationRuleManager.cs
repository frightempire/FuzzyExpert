using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRuleManager.Interfaces
{
    public interface IImplicationRuleManager
    {
        List<ImplicationRule> ImplicationRules { get; }
    }
}