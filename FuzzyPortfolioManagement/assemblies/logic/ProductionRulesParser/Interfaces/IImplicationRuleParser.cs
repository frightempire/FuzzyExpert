using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleParser
    {
        List<string> GetStatementParts(ref string implicationRule);

        ImplicationRuleStrings ExtractStatementParts(string implicationRule);

        UnaryStatement ParseUnaryStatement(string statement);
    }
}
