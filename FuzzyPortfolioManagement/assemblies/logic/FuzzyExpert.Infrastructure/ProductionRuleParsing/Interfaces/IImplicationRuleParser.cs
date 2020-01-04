using System.Collections.Generic;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Entities;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces
{
    public interface IImplicationRuleParser
    {
        List<string> ParseImplicationRule(ref string implicationRule);

        List<string> ParseStatementCombination(string statement);

        ImplicationRuleStrings ExtractStatementParts(string implicationRule);

        UnaryStatement ParseUnaryStatement(string statement);
    }
}
