using System;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.Extensions;

namespace FuzzyExpert.Core.Entities
{
    public class UnaryStatement
    {
        public UnaryStatement(string leftOperand, ComparisonOperation comparisonOperation, string rightOperand)
        {
            if (string.IsNullOrWhiteSpace(leftOperand)) throw new ArgumentNullException(nameof(leftOperand));
            if (string.IsNullOrWhiteSpace(rightOperand)) throw new ArgumentNullException(nameof(rightOperand));

            LeftOperand = leftOperand;
            ComparisonOperation = comparisonOperation;
            RightOperand = rightOperand;
        }

        public string LeftOperand { get; }

        public ComparisonOperation ComparisonOperation { get; }

        public string RightOperand { get; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{LeftOperand} {ComparisonOperation.GetDescription()} {RightOperand}";
        }
    }
}