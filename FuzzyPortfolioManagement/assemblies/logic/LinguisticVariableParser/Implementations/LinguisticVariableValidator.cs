using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using LinguisticVariableParser.Interfaces;
using MembershipFunctionParser.Interfaces;

namespace LinguisticVariableParser.Implementations
{
    public class LinguisticVariableValidator : ILinguisticVariableValidator
    {
        private readonly IMembershipFunctionValidator _membershipFunctionValidator;

        public LinguisticVariableValidator(IMembershipFunctionValidator membershipFunctionValidator)
        {
            ExceptionAssert.IsNull(membershipFunctionValidator);
            _membershipFunctionValidator = membershipFunctionValidator;
        }

        public ValidationOperationResult ValidateLinguisticVariable(string linguisticVariable)
        {
            ValidationOperationResult validationOperationResult = new ValidationOperationResult();

            if (linguisticVariable.Contains(" "))
                validationOperationResult.AddMessage("Linguistic variable string is not valid: haven't been preprocessed");

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
            {
                validationOperationResult.AddMessage("Linguistic variable string is not valid: incorrect brackets for membership functions");
                return validationOperationResult;
            }

            int firstColonPosition = linguisticVariable.IndexOf(':');
            int secondColonPosition = linguisticVariable.IndexOf(':', firstColonPosition + 1);
            if (!ColunsInLinguisticVariablePlacedCorrectly(brackets[0].Position, firstColonPosition, secondColonPosition))
                validationOperationResult.AddMessage("Linguistic variable string is not valid: colon delimeters placed incorrectly");

            string membershipFunctionsPart = linguisticVariable.Substring(
                brackets[0].Position + 1, brackets[1].Position - brackets[0].Position - 1);
            ValidationOperationResult validationOperationResultOfMembershipFunctionsPart =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionsPart);

            List<string> membershipPartValidationMessages =
                validationOperationResultOfMembershipFunctionsPart.Messages;
            if (membershipPartValidationMessages.Count != 0)
                validationOperationResult.AddMessages(membershipPartValidationMessages);

            return validationOperationResult;
        }

        private bool ColunsInLinguisticVariablePlacedCorrectly(
            int openingBracketPosition,
            int firstColonPosition,
            int secondColonPosition)
        {
            return firstColonPosition < openingBracketPosition &&
                   secondColonPosition < openingBracketPosition;
        }
    }
}