using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleParser
    {
        List<string> ParseImplicationRule(ref string implicationRule);

        List<string> ParseStatementCombination(string statement);

        ImplicationRuleStrings ExtractStatementParts(string implicationRule);

        UnaryStatement ParseUnaryStatement(string statement);
    }
}
