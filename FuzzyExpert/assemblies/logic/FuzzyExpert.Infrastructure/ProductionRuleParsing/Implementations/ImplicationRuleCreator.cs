using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations
{
    public class ImplicationRuleCreator : IImplicationRuleCreator
    {
        private readonly IImplicationRuleParser _implicationRuleParser;

        public ImplicationRuleCreator(IImplicationRuleParser implicationRuleParser)
        {
            _implicationRuleParser = implicationRuleParser ?? throw new ArgumentNullException(nameof(implicationRuleParser));
        }

        public ImplicationRule CreateImplicationRuleEntity(string implicationRule)
        {
            var implicationRuleStrings = _implicationRuleParser.ExtractStatementParts(implicationRule);
            var ifStatement = implicationRuleStrings.IfStatement;
            var thenStatement = implicationRuleStrings.ThenStatement;
            var ifStatementParts = _implicationRuleParser.ParseImplicationRule(ref ifStatement);
            var thenStatementParts = _implicationRuleParser.ParseStatementCombination(thenStatement);

            var ifStatementCombination = new List<StatementCombination>();
            foreach (var ifStatementPart in ifStatementParts)
            {
                var ifUnaryStatementStrings = _implicationRuleParser.ParseStatementCombination(ifStatementPart);
                var ifUnaryStatements = ifUnaryStatementStrings
                    .Select(ifUnaryStatementString => _implicationRuleParser.ParseUnaryStatement(ifUnaryStatementString))
                    .ToList();
                ifStatementCombination.Add(new StatementCombination(ifUnaryStatements));
            }
            
            var thenUnaryStatements = thenStatementParts
                .Select(thenStatementPart => _implicationRuleParser.ParseUnaryStatement(thenStatementPart))
                .ToList();
            var thenStatementCombination = new StatementCombination(thenUnaryStatements);

            return new ImplicationRule(ifStatementCombination, thenStatementCombination);
        }
    }
}