using System;
using CommonLogic;
using CommonLogic.Extensions;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Enums;
using LinguisticVariableParser.Interfaces;

namespace LinguisticVariableParser.Implementations
{
    public class LinguisticVariableCreator : ILinguisticVariableCreator
    {
        private readonly IMembershipFunctionCreator _membershipFunctionCreator;

        public LinguisticVariableCreator(IMembershipFunctionCreator membershipFunctionCreator)
        {
            ExceptionAssert.IsNull(membershipFunctionCreator);
            _membershipFunctionCreator = membershipFunctionCreator;
        }

        public LinguisticVariable CreateLinguisticVariableEntity(LinguisticVariableStrings linguisticVariableStrings)
        {
            DataOriginType dataOriginType = linguisticVariableStrings.DataOrigin.ToEnum<DataOriginType>();
            bool isInitial = dataOriginType == DataOriginType.Initial;

            MembershipFunctionList membershipFunctions = new MembershipFunctionList();
            foreach (MembershipFunctionStrings membershipFunctionStrings in linguisticVariableStrings.MembershipFunctions)
            {
                MembershipFunctionType functionType = membershipFunctionStrings.MembershipFunctionType.ToEnum<MembershipFunctionType>();
                var membershipFunction = _membershipFunctionCreator.CreateMembershipFunctionEntity(
                    functionType, membershipFunctionStrings.MembershipFunctionName, membershipFunctionStrings.MembershipFunctionValues);
                membershipFunctions.Add(membershipFunction);
            }

            return new LinguisticVariable(linguisticVariableStrings.VariableName, membershipFunctions, isInitial);
        }
    }
}