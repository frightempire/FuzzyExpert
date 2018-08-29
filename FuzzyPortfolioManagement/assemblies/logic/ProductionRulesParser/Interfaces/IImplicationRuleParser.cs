using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleParser
    {
        List<string> DivideImplicationRule(string implicationRule);

        ImplicationRule CreateImplicationRuleEntity(string implicationRule);
    }
}