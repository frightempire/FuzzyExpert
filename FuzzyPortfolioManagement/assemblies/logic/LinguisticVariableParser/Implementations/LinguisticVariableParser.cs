using System.Collections.Generic;
using CommonLogic;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Interfaces;

namespace LinguisticVariableParser.Implementations
{
    public class LinguisticVariableParser: ILinguisticVariableParser
    {
        private readonly IMembershipFunctionParser _membershipFunctionParser;

        public LinguisticVariableParser(IMembershipFunctionParser membershipFunctionParser)
        {
            ExceptionAssert.IsNull(membershipFunctionParser);
            _membershipFunctionParser = membershipFunctionParser;
        }

        public LinguisticVariableStrings ParseLinguisticVariable(string linguisticVariable)
        {
            int firstColunPosition = linguisticVariable.IndexOf(':');
            int secondColunPosition = linguisticVariable.IndexOf(':', firstColunPosition + 1);

            string linguisticVariableNameString = linguisticVariable.Substring(0, firstColunPosition);
            string linguisticVariableDataOriginString =
                linguisticVariable.Substring(firstColunPosition + 1, secondColunPosition - firstColunPosition - 1);

            int openingBracketPosition = linguisticVariable.IndexOf('[');
            int closingBracketPosition = linguisticVariable.IndexOf(']');

            string membershipFunctionsPart = linguisticVariable.Substring(
                openingBracketPosition + 1, closingBracketPosition - openingBracketPosition - 1);

            List<MembershipFunctionStrings> membershipFunctionStringsList = _membershipFunctionParser.ParseMembershipFunctions(membershipFunctionsPart);

            return new LinguisticVariableStrings(linguisticVariableNameString, linguisticVariableDataOriginString, membershipFunctionStringsList);
        }
    }
}
