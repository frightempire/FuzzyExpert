using System;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations
{
    public class LinguisticVariableCreator : ILinguisticVariableCreator
    {
        private readonly IMembershipFunctionCreator _membershipFunctionCreator;

        public LinguisticVariableCreator(IMembershipFunctionCreator membershipFunctionCreator)
        {
            _membershipFunctionCreator = membershipFunctionCreator ?? throw new ArgumentNullException(nameof(membershipFunctionCreator));
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