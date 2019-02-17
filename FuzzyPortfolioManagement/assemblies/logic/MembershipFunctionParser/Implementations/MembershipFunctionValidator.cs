using System;
using System.Collections.Generic;
using System.Linq;
using CommonLogic.Entities;
using MembershipFunctionParser.Interfaces;

namespace MembershipFunctionParser.Implementations
{
    public class MembershipFunctionValidator : IMembershipFunctionValidator
    {
        public ValidationOperationResult ValidateMembershipFunctionsPart(string membershipFunctionsPart)
        {
            ValidationOperationResult validationOperationResult = new ValidationOperationResult();

            List<StringCharacter> brackets = new List<StringCharacter>();
            for (var i = 0; i < membershipFunctionsPart.Length; i++)
            {
                char character = membershipFunctionsPart[i];
                if (character == '(' || character == ')')
                    brackets.Add(new StringCharacter(character, i));
            }
            int bracketsCount = brackets.Count;

            if (bracketsCount < 4)
            {
                validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: no enough brackets");
                return validationOperationResult;
            }
            if (bracketsCount % 2 != 0)
            {
                validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: odd count of brackets");
                return validationOperationResult;
            }
            if (brackets.First().Symbol != '(' || brackets.Last().Symbol != ')')
            {
                validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: first or last brackets are wrong");
                return validationOperationResult;
            }

            List<Tuple<StringCharacter, StringCharacter>> correspondingBracketsList = new List<Tuple<StringCharacter, StringCharacter>>();
            for (int i = 0; i < bracketsCount; i = i + 2)
            {
                if (brackets[i].Symbol != '(' || brackets[i + 1].Symbol != ')')
                {
                    validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: mismatching brackets");
                    return validationOperationResult;
                }

                correspondingBracketsList.Add(Tuple.Create(brackets[i], brackets[i + 1]));
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
                {
                    validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: incorrect colun placement");
                    return validationOperationResult;
                }

                if (correspondingBrackets.Item2.Position == correspondingBrackets.Item1.Position + 1)
                {
                    validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: empty brackets");
                    return validationOperationResult;
                }
                if (i != correspondingBracketsList.Count - 1 &&
                    membershipFunctionsPart[correspondingBrackets.Item2.Position + 1] != '|')
                {
                    validationOperationResult.AddMessage("Linguistic variable membership functions are not valid: missing delimiter between functions");
                    return validationOperationResult;
                }
            }

            return validationOperationResult;
        }

        private bool ColunsInMembershipFunctionsPlacedCorrectly(
            string membershipFunctionsPart,
            int openingBracketPosition,
            int firstColonPosition,
            int secondColonPosition)
        {
            return firstColonPosition >= 1 &&
                   secondColonPosition >= 3 &&
                   firstColonPosition < openingBracketPosition &&
                   secondColonPosition < openingBracketPosition &&
                   membershipFunctionsPart[firstColonPosition - 1] != '|' &&
                   membershipFunctionsPart[firstColonPosition - 1] != ')' &&
                   membershipFunctionsPart[secondColonPosition - 1] != ':' &&
                   membershipFunctionsPart[secondColonPosition + 1] == '(';
        }
    }
}