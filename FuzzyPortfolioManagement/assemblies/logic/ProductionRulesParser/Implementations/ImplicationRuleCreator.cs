using System;
using System.Collections.Generic;
using CommonLogic;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Interfaces;

namespace ProductionRulesParser.Implementations
{
    public class ImplicationRuleCreator : IImplicationRuleCreator
    {
        private readonly IImplicationRuleParser _implicationRuleParser;
        private readonly IImplicationRulePreProcessor _implicationRulePreProcessor;

        public ImplicationRuleCreator(
            IImplicationRuleParser implicationRuleParser,
            IImplicationRulePreProcessor implicationRulePreProcessor)
        {
            ExceptionAssert.IsNull(implicationRuleParser);
            ExceptionAssert.IsNull(implicationRulePreProcessor);

            _implicationRuleParser = implicationRuleParser;
            _implicationRulePreProcessor = implicationRulePreProcessor;
        }

        public ImplicationRuleStrings DivideImplicationRule(string implicationRule)
        {
            _implicationRulePreProcessor.ValidateImplicationRule(implicationRule);

            string preProcessedImplicationRule = _implicationRulePreProcessor.PreProcessImplicationRule(implicationRule);
            return _implicationRuleParser.ExtractStatementParts(preProcessedImplicationRule);
        }

        public ImplicationRule CreateImplicationRuleEntity(ImplicationRuleStrings implicationRuleStrings)
        {
            string ifStatement = implicationRuleStrings.IfStatement;
            string thenStatement = implicationRuleStrings.ThenStatement;
            List<string> ifStatementParts = _implicationRuleParser.ParseImplicationRule(ref ifStatement);
            List<string> thenStatementParts = _implicationRuleParser.ParseStatementCombination(thenStatement);

            List<StatementCombination> ifStatementCombination = new List<StatementCombination>();
            foreach (string ifStatementPart in ifStatementParts)
            {
                List<string> ifUnaryStatementStrings = _implicationRuleParser.ParseStatementCombination(ifStatementPart);

                List<UnaryStatement> ifUnaryStatements = new List<UnaryStatement>();
                foreach (string ifUnaryStatementString in ifUnaryStatementStrings)
                {
                    UnaryStatement ifUnaryStatement = _implicationRuleParser.ParseUnaryStatement(ifUnaryStatementString);
                    ifUnaryStatements.Add(ifUnaryStatement);
                }

                ifStatementCombination.Add(new StatementCombination(ifUnaryStatements));
            }
            
            List<UnaryStatement> thenUnaryStatements = new List<UnaryStatement>();
            foreach (string thenStatementPart in thenStatementParts)
            {
                UnaryStatement thenUnaryStatement = _implicationRuleParser.ParseUnaryStatement(thenStatementPart);
                thenUnaryStatements.Add(thenUnaryStatement);
            }
            StatementCombination thenStatementCombination = new StatementCombination(thenUnaryStatements);

            return new ImplicationRule(ifStatementCombination, thenStatementCombination);
        }
    }
}