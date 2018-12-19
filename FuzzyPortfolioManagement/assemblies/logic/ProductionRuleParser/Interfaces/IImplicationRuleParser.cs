using System.Collections.Generic;
using ProductionRuleParser.Entities;

namespace ProductionRuleParser.Interfaces
{
    public interface IImplicationRuleParser
    {
        List<string> ParseImplicationRule(ref string implicationRule);

        List<string> ParseStatementCombination(string statement);

        ImplicationRuleStrings ExtractStatementParts(string implicationRule);

        UnaryStatement ParseUnaryStatement(string statement);
    }
}
