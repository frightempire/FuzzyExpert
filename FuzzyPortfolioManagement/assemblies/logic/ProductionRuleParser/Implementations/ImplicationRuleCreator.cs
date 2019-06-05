using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Interfaces;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Interfaces;

namespace ProductionRuleParser.Implementations
{
    public class ImplicationRuleCreator : IImplicationRuleCreator
    {
        private readonly IImplicationRuleParser _implicationRuleParser;

        public ImplicationRuleCreator(IImplicationRuleParser implicationRuleParser)
        {
            ExceptionAssert.IsNull(implicationRuleParser);
            _implicationRuleParser = implicationRuleParser;
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