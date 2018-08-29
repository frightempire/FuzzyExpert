using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleHelper
    {
        List<string> GetStatementParts(ref string implicationRule);

        void ValidateImplicationRule(string implicationRule);

        string PreProcessImplicationRule(string implicationRule);

        ImplicationRuleStrings ExtractStatementParts(string implicationRule);
    }
}
