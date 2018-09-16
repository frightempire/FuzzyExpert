using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleCreator
    {
        ImplicationRuleStrings DivideImplicationRule(string implicationRule);

        ImplicationRule CreateImplicationRuleEntity(ImplicationRuleStrings implicationRuleStrings);
    }
}