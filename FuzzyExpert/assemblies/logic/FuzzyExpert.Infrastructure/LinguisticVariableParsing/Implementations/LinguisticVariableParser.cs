using System;
using System.Collections.Generic;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
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

        public LinguisticVariableStrings ParseLinguisticVariable(string linguisticVariable)
        {
            int firstColumnPosition = linguisticVariable.IndexOf(':');
            int secondColumnPosition = linguisticVariable.IndexOf(':', firstColumnPosition + 1);

            string linguisticVariableNameString = linguisticVariable.Substring(0, firstColumnPosition);
            string linguisticVariableDataOriginString =
                linguisticVariable.Substring(firstColumnPosition + 1, secondColumnPosition - firstColumnPosition - 1);

            int openingBracketPosition = linguisticVariable.IndexOf('[');
            int closingBracketPosition = linguisticVariable.IndexOf(']');

            string membershipFunctionsPart = linguisticVariable.Substring(
                openingBracketPosition + 1, closingBracketPosition - openingBracketPosition - 1);

            List<MembershipFunctionStrings> membershipFunctionStringsList = _membershipFunctionParser.ParseMembershipFunctions(membershipFunctionsPart);

            return new LinguisticVariableStrings(linguisticVariableNameString, linguisticVariableDataOriginString, membershipFunctionStringsList);
        }
    }
}