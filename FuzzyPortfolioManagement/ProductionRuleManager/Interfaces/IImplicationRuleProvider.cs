using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRuleManager.Interfaces
{
    public interface IImplicationRuleProvider
    {
        List<ImplicationRule> GetImplicationRules();
    }
}
