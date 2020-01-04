using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations
{
    public class ImplicationRuleParser : IImplicationRuleParser
    {
        // Simplifies implication rule string. As the result we have list of statements divided by OR.
        public List<string> ParseImplicationRule(ref string implicationRuleString)
        {
            List<string> ruleParts = new List<string>();
            List<string> implicationRules = new List<string>();
            string appendingString = string.Empty;

            while (implicationRuleString.Length != 0)
            {
                int iterator = 0;
                char currentCharacter = implicationRuleString[iterator];
                implicationRuleString = implicationRuleString.Remove(iterator, 1);
                switch (currentCharacter)
                {
                    case '(':
                        ruleParts.AddRange(ParseImplicationRule(ref implicationRuleString));
                        for (int i = 0; i < ruleParts.Count; i++)
                            ruleParts[i] = appendingString + ruleParts[i];
                        appendingString = string.Empty;
                        continue;
                    case ')':
                        if (ruleParts.Any())
                        {                          
                            for (int i = 0; i < ruleParts.Count; i++)
                                ruleParts[i] += appendingString;
                            implicationRules.AddRange(ruleParts);
                        }
                        else if (!string.IsNullOrEmpty(appendingString))
                            implicationRules.Add(appendingString);

                        return implicationRules;
                    case '|':
                        if (ruleParts.Any())
                        {
                            for (int i = 0; i < ruleParts.Count; i++)
                                ruleParts[i] += appendingString;
                            implicationRules.AddRange(ruleParts);
                            ruleParts = new List<string>();
                        }
                        else if (!string.IsNullOrEmpty(appendingString))
                            implicationRules.Add(appendingString);

                        appendingString = string.Empty;
                        continue;
                    default:
                        appendingString += currentCharacter;
                        continue;
                }
            }

            if (ruleParts.Any())
            {
                for (int i = 0; i < ruleParts.Count; i++)
                    ruleParts[i] += appendingString;
                implicationRules.AddRange(ruleParts);
            }
            else if (!string.IsNullOrEmpty(appendingString))
                implicationRules.Add(appendingString);

            return implicationRules;
        }

        public List<string> ParseStatementCombination(string statement)
        {
            if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentNullException(nameof(statement));
            return statement.Replace("(", "").Replace(")", "").Split('&').ToList();
        }

        public ImplicationRuleStrings ExtractStatementParts(string implicationRule)
        {
            if (string.IsNullOrWhiteSpace(implicationRule)) throw new ArgumentNullException(nameof(implicationRule));

            int indexOfDelimiter = implicationRule.IndexOf(")THEN(", StringComparison.Ordinal);

            if (indexOfDelimiter == -1)
                throw new ArgumentException("Invalid implication rule");

            string ifStatement = implicationRule.Substring(2, indexOfDelimiter - 1);
            string thenStatement = implicationRule.Substring(indexOfDelimiter + 5);
            return new ImplicationRuleStrings(ifStatement, thenStatement);
        }

        public UnaryStatement ParseUnaryStatement(string statement)
        {
            if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentNullException(nameof(statement));

            int indexOfLess = statement.IndexOf('<');
            int indexOfLessEquals = statement.IndexOf("<=", StringComparison.Ordinal);
            int indexOfGrater = statement.IndexOf('>');
            int indexOfGraterEquals = statement.IndexOf(">=", StringComparison.Ordinal);
            int indexOfEqual = statement.IndexOf('=');
            int indexOfNotEqual = statement.IndexOf("!=", StringComparison.Ordinal);

            ComparisonOperation comparisonOperation;
            string leftOperand;
            string rightOperand;

            if (indexOfLess != -1)
            {
                if (indexOfLessEquals != -1)
                {
                    comparisonOperation = ComparisonOperation.LessOrEqual;
                    leftOperand = statement.Substring(0, indexOfLessEquals);
                    rightOperand = statement.Substring(indexOfLessEquals + 2);
                }
                else
                {
                    comparisonOperation = ComparisonOperation.Less;
                    leftOperand = statement.Substring(0, indexOfLess);
                    rightOperand = statement.Substring(indexOfLess + 1);
                }
            }
            else if (indexOfGrater != -1)
            {
                if (indexOfGraterEquals != -1)
                {
                    comparisonOperation = ComparisonOperation.GreaterOrEqual;
                    leftOperand = statement.Substring(0, indexOfGraterEquals);
                    rightOperand = statement.Substring(indexOfGraterEquals + 2);
                }
                else
                {
                    comparisonOperation = ComparisonOperation.Greater;
                    leftOperand = statement.Substring(0, indexOfGrater);
                    rightOperand = statement.Substring(indexOfGrater + 1);
                }
            }
            else if (indexOfEqual != -1)
            {
                if (indexOfNotEqual != -1)
                {
                    comparisonOperation = ComparisonOperation.NotEqual;
                    leftOperand = statement.Substring(0, indexOfNotEqual);
                    rightOperand = statement.Substring(indexOfNotEqual + 2);
                }
                else
                {
                    comparisonOperation = ComparisonOperation.Equal;
                    leftOperand = statement.Substring(0, indexOfEqual);
                    rightOperand = statement.Substring(indexOfEqual + 1);
                }
            }
            else
            {
                throw new ArgumentException("Statement doesn't contain comparison operators.");
            }

            return new UnaryStatement(leftOperand, comparisonOperation, rightOperand);
        }
    }
}