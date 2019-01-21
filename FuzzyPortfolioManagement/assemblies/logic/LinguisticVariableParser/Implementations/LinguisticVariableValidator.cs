using System;
using System.Collections.Generic;
using System.Linq;
using CommonLogic.Entities;
using LinguisticVariableParser.Interfaces;

namespace LinguisticVariableParser.Implementations
{
    public class LinguisticVariableValidator : ILinguisticVariableValidator
    {
        public void ValidateLinguisticVariable(string linguisticVariable)
        {
            if (linguisticVariable.Contains(" "))
                throw new ArgumentException("Linguistic variable string is not valid: haven't been preprocessed");

            List<StringCharacter> brackets = new List<StringCharacter>();
            for (var i = 0; i < linguisticVariable.Length; i++)
            {
                char character = linguisticVariable[i];
                if (character == '[' || character == ']')
                    brackets.Add(new StringCharacter(character, i));
            }
            int bracketsCount = brackets.Count;
            
            if (bracketsCount != 2 ||
                brackets[0].Symbol != '[' ||
                brackets[1].Symbol != ']')
                throw new ArgumentException("Linguistic variable string is not valid: incorrect brackets for membership functions");

            int firstColonPosition = linguisticVariable.IndexOf(':');
            int secondColonPosition = linguisticVariable.IndexOf(':', firstColonPosition + 1);
            if (!ColunsInLinguisticVariablePlacedCorrectly(brackets[0].Position, firstColonPosition, secondColonPosition))
                throw new ArgumentException("Linguistic variable string is not valid: colon delimeters placed incorrectly");

            string membershipFunctionsPart = linguisticVariable.Substring(
                brackets[0].Position + 1, brackets[1].Position - brackets[0].Position - 1);

            ValidateMembershipFunctions(membershipFunctionsPart);
        }

        private void ValidateMembershipFunctions(string membershipFunctionsPart)
        {
            List<StringCharacter> brackets = new List<StringCharacter>();
            for (var i = 0; i < membershipFunctionsPart.Length; i++)
            {
                char character = membershipFunctionsPart[i];
                if (character == '(' || character == ')')
                    brackets.Add(new StringCharacter(character, i));
            }
            int bracketsCount = brackets.Count;

            if (bracketsCount < 4)
                throw new ArgumentException("Linguistic variable membership functions are not valid: no enough brackets");
            if (bracketsCount % 2 != 0)
                throw new ArgumentException("Linguistic variable membership functions are not valid: odd count of brackets");
            if (brackets.First().Symbol != '(' || brackets.Last().Symbol != ')')
                throw new ArgumentException("Linguistic variable membership functions are not valid: first or last brackets are wrong");

            List<Tuple<StringCharacter, StringCharacter>> correspondingBracketsList = new List<Tuple<StringCharacter, StringCharacter>>();
            for (int i = 0; i < bracketsCount; i = i + 2)
            {
                if (brackets[i].Symbol != '(' || brackets[i+1].Symbol != ')')
                    throw new ArgumentException("Linguistic variable membership functions are not valid: mismatching brackets");

                correspondingBracketsList.Add(Tuple.Create(brackets[i], brackets[i+1]));
            }

            int beginningColonPosition = 0;
            for (var i = 0; i < correspondingBracketsList.Count; i++)
            {
                int firstColonPosition = membershipFunctionsPart.IndexOf(':', beginningColonPosition);
                int secondColonPosition = membershipFunctionsPart.IndexOf(':', firstColonPosition + 1);
                beginningColonPosition = secondColonPosition + 1;

                var correspondingBrackets = correspondingBracketsList[i];
                if (!ColunsInMembershipFunctionsPlacedCorrectly(
                    membershipFunctionsPart,
                    correspondingBrackets.Item1.Position,
                    firstColonPosition,
                    secondColonPosition))
                    throw new ArgumentException("Linguistic variable membership functions are not valid: incorrect colun placement");

                if (correspondingBrackets.Item2.Position == correspondingBrackets.Item1.Position + 1)
                    throw new ArgumentException("Linguistic variable membership functions are not valid: empty brackets");

                if (i != correspondingBracketsList.Count - 1 &&
                    correspondingBrackets.Item2.Position + 1 != '|')
                    throw new ArgumentException("Linguistic variable membership functions are not valid: missing delimiter between functions");
            }

            // parse

            //List<StringCharacter> separators = new List<StringCharacter>();
            //for (var i = 0; i < membershipFunctionsPart.Length; i++)
            //{
            //    char character = membershipFunctionsPart[i];
            //    if (character == '|')
            //        separators.Add(new StringCharacter(character, i));
            //}
            //int separatorsCount = separators.Count;

            //List<string> membershipFunctions = new List<string>();
            //for (var i = 0; i < separatorsCount; i++)
            //{
            //    if (i == 0)
            //    {
            //        membershipFunctions.Add(membershipFunctionsPart.Substring(0, separators[i].Position - 1));
            //    }
            //    else if (i == separatorsCount - 1)
            //    {
            //        membershipFunctions.Add(membershipFunctionsPart.Substring(separators[i-1].Position + 1, separators[i].Position - separators[i - 1].Position));
            //        membershipFunctions.Add(membershipFunctionsPart.Substring(separators[i].Position + 1, membershipFunctionsPart.Length));
            //    }
            //    else
            //    {
            //        membershipFunctions.Add(membershipFunctionsPart.Substring(separators[i].Position + 1, separators[i+1].Position - separators[i].Position));
            //    }
            //}
        }

        private bool ColunsInMembershipFunctionsPlacedCorrectly(
            string membershipFunctionsPart,
            int openingBracketPosition,
            int firstColonPosition,
            int secondColonPosition)
        {
            return firstColonPosition != -1 &&
                   secondColonPosition != -1 &&
                   firstColonPosition < openingBracketPosition &&
                   secondColonPosition < openingBracketPosition &&
                   membershipFunctionsPart[firstColonPosition - 1] != 0 &&
                   membershipFunctionsPart[firstColonPosition - 1] != '|' &&
                   membershipFunctionsPart[firstColonPosition - 1] != ')' &&
                   membershipFunctionsPart[secondColonPosition - 1] != ':' &&
                   membershipFunctionsPart[secondColonPosition + 1] == '(';
        }

        private bool ColunsInLinguisticVariablePlacedCorrectly(
            int openingBracketPosition,
            int firstColonPosition,
            int secondColonPosition)
        {
            return firstColonPosition > openingBracketPosition ||
                   secondColonPosition > openingBracketPosition;
        }
    }
}