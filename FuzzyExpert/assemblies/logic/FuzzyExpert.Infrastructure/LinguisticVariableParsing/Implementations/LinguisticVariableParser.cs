using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations
{
    public class LinguisticVariableParser: ILinguisticVariableParser
    {
        private readonly IMembershipFunctionParser _membershipFunctionParser;

        public LinguisticVariableParser(IMembershipFunctionParser membershipFunctionParser)
        {
            _membershipFunctionParser = membershipFunctionParser ?? throw new ArgumentNullException(nameof(membershipFunctionParser));
        }

        public List<LinguisticVariableStrings> ParseLinguisticVariable(string linguisticVariable)
        {
            var firstColumnPosition = linguisticVariable.IndexOf(':');
            var secondColumnPosition = linguisticVariable.IndexOf(':', firstColumnPosition + 1);

            var linguisticVariableNameStrings = linguisticVariable.Substring(1, firstColumnPosition - 2).Split(',').ToList();
            var linguisticVariableDataOriginString = linguisticVariable.Substring(firstColumnPosition + 1, secondColumnPosition - firstColumnPosition - 1);
            var membershipFunctionsPart = linguisticVariable.Substring(secondColumnPosition + 2, linguisticVariable.Length - secondColumnPosition - 3);
            var membershipFunctionStringsList = _membershipFunctionParser.ParseMembershipFunctions(membershipFunctionsPart);

            return linguisticVariableNameStrings.Select(lvns =>
                new LinguisticVariableStrings(lvns, linguisticVariableDataOriginString, membershipFunctionStringsList))
                .ToList();
        }
    }
}