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
            List<string> ifStatementParts = _implicationRuleParser.GetStatementParts(ref ifStatement);

            List<UnaryStatement> ifUnaryStatements = new List<UnaryStatement>();
            foreach (string ifStatementPart in ifStatementParts)
            {
                UnaryStatement ifUnaryStatement = _implicationRuleParser.ParseUnaryStatement(ifStatementPart);
                ifUnaryStatements.Add(ifUnaryStatement);
            }

            UnaryStatement thenUnaryStatement = _implicationRuleParser.ParseUnaryStatement(thenStatement);

            return new ImplicationRule(ifUnaryStatements, thenUnaryStatement);
        }
    }
}