using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyExpert.Core.Entities
{
    public class ImplicationRule
    {
        public ImplicationRule(List<StatementCombination> ifStatement, StatementCombination thenStatement)
        {
            IfStatement = ifStatement ?? throw new ArgumentNullException(nameof(ifStatement));
            ThenStatement = thenStatement ?? throw new ArgumentNullException(nameof(thenStatement));
        }

        // Divided by OR
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
                    ifStatementStringBuilder.Append(unaryStatement);
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
                thenStatementStringBuilder.Append(unaryStatement);
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