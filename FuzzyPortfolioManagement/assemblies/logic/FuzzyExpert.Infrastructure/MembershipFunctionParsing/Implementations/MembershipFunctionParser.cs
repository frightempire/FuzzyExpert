using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations
{
    public class MembershipFunctionParser : IMembershipFunctionParser
    {
        public List<MembershipFunctionStrings> ParseMembershipFunctions(string membershipFunctionsPart)
        {
            List<string> membershipFunctions = ExtractMembershipFunctionsStrings(membershipFunctionsPart);
            List<MembershipFunctionStrings> membershipFunctionStringsList = ParseMembershipFunctionStrings(membershipFunctions);
            return membershipFunctionStringsList;
        }

        private List<MembershipFunctionStrings> ParseMembershipFunctionStrings(List<string> membershipFunctions)
        {
            List<MembershipFunctionStrings> membershipFunctionStringsList = new List<MembershipFunctionStrings>();
            foreach (string membershipFunction in membershipFunctions)
            {
                int firstColumnPosition = membershipFunction.IndexOf(':');
                int secondsColumnPosition = membershipFunction.IndexOf(':', firstColumnPosition + 1);

                string membershipFunctionName = membershipFunction.Substring(0, firstColumnPosition);
                string membershipFunctionType =
                    membershipFunction.Substring(firstColumnPosition + 1, secondsColumnPosition - firstColumnPosition - 1);

                int openingBracketPosition = membershipFunction.IndexOf('(');
                int closingBracketPosition = membershipFunction.IndexOf(')');

                string membershipFunctionValuesPart = membershipFunction.Substring(
                    openingBracketPosition + 1,
                    closingBracketPosition - openingBracketPosition - 1);

                List<string> values = membershipFunctionValuesPart.Split(',').ToList();
                List<double> membershipFunctionValues = new List<double>();
                foreach (string value in values)
                {
                    membershipFunctionValues.Add(double.Parse(value));
                }

                membershipFunctionStringsList.Add(
                    new MembershipFunctionStrings(membershipFunctionName, membershipFunctionType, membershipFunctionValues));
            }
            return membershipFunctionStringsList;
        }

        private List<string> ExtractMembershipFunctionsStrings(string membershipFunctionsPart)
        {
            List<StringCharacter> separators = new List<StringCharacter> {new StringCharacter(' ', 0)};
            for (var i = 0; i < membershipFunctionsPart.Length; i++)
            {
                char character = membershipFunctionsPart[i];
                if (character == '|')
                    separators.Add(new StringCharacter(character, i));
            }
            separators.Add(new StringCharacter(' ', membershipFunctionsPart.Length - 1));
            int separatorsCount = separators.Count;

            List<string> membershipFunctions = new List<string>();
            for (var i = 0; i < separatorsCount - 1; i++)
            {
                int startingPosition;
                int length;
                if (i == 0)
                {
                    startingPosition = 0;
                    length = separators[i + 1].Position;
                }
                else if (i == separatorsCount - 2)
                {
                    startingPosition = separators[i].Position + 1;
                    length = separators[i + 1].Position - separators[i].Position;
                }
                else
                {
                    startingPosition = separators[i].Position + 1;
                    length = separators[i + 1].Position - separators[i].Position - 1;
                }
                membershipFunctions.Add(membershipFunctionsPart.Substring(startingPosition, length));
            }
            return membershipFunctions;
        }
    }
}