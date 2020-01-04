using System;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Entities
{
    public class ImplicationRuleStrings
    {
        public ImplicationRuleStrings(string ifStatement, string thenStatement)
        {
            if (string.IsNullOrWhiteSpace(ifStatement)) throw new ArgumentNullException(nameof(ifStatement));
            if (string.IsNullOrWhiteSpace(thenStatement)) throw new ArgumentNullException(nameof(thenStatement));

            IfStatement = ifStatement;
            ThenStatement = thenStatement;
        }

        public string IfStatement { get; }

        public string ThenStatement { get; }
    }
}