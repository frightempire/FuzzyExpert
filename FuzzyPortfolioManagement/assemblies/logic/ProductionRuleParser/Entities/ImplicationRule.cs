using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLogic;
using CommonLogic.Extensions;

namespace ProductionRuleParser.Entities
{
    public class ImplicationRule
    {
        public ImplicationRule(List<StatementCombination> ifStatement, StatementCombination thenStatement)
        {
            ExceptionAssert.IsNull(ifStatement);
            ExceptionAssert.IsNull(thenStatement);

            IfStatement = ifStatement;
            ThenStatement = thenStatement;
        }

        public List<StatementCombination> IfStatement { get; }

        public StatementCombination ThenStatement { get; }

        public override string ToString()
        {
            StringBuilder resultingStringBuilder = new StringBuilder();

            resultingStringBuilder.Append("IF ");
            resultingStringBuilder.Append("(");

            StringBuilder ifStatementStringBuilder = new StringBuilder();

            StatementCombination lastStatementCombination = IfStatement.Last();
            foreach (StatementCombination statementCombination in IfStatement)
            {
                if (statementCombination.UnaryStatements.Count > 1 && IfStatement.Count > 1)
                    ifStatementStringBuilder.Append("(");

                UnaryStatement lastIfUnaryStatement = statementCombination.UnaryStatements.Last();
                foreach (UnaryStatement unaryStatement in statementCombination.UnaryStatements)
                {
                    ifStatementStringBuilder.Append(unaryStatement.LeftOperand);
                    ifStatementStringBuilder.Append(" ");
                    ifStatementStringBuilder.Append(unaryStatement.ComparisonOperation.GetDescription());
                    ifStatementStringBuilder.Append(" ");
                    ifStatementStringBuilder.Append(unaryStatement.RightOperand);

                    if (unaryStatement != lastIfUnaryStatement)
                        ifStatementStringBuilder.Append(" & ");
                }

                if (statementCombination.UnaryStatements.Count > 1 && IfStatement.Count > 1)
                    ifStatementStringBuilder.Append(")");

                if (statementCombination != lastStatementCombination)
                    ifStatementStringBuilder.Append(" | ");
            }
            string ifStatementString = ifStatementStringBuilder.ToString();

            resultingStringBuilder.Append(ifStatementString);
            resultingStringBuilder.Append(")");

            resultingStringBuilder.Append(" THEN ");
            resultingStringBuilder.Append("(");

            StringBuilder thenStatementStringBuilder = new StringBuilder();
            UnaryStatement lastThenUnaryStatement = ThenStatement.UnaryStatements.Last();
            foreach (UnaryStatement unaryStatement in ThenStatement.UnaryStatements)
            {
                thenStatementStringBuilder.Append(unaryStatement.LeftOperand);
                thenStatementStringBuilder.Append(" ");
                thenStatementStringBuilder.Append(unaryStatement.ComparisonOperation.GetDescription());
                thenStatementStringBuilder.Append(" ");
                thenStatementStringBuilder.Append(unaryStatement.RightOperand);

                if (unaryStatement != lastThenUnaryStatement)
                    thenStatementStringBuilder.Append(" & ");
            }
            string thenStatementString = thenStatementStringBuilder.ToString();
            resultingStringBuilder.Append(thenStatementString);
            resultingStringBuilder.Append(")");

            return resultingStringBuilder.ToString();
        }
    }
}
