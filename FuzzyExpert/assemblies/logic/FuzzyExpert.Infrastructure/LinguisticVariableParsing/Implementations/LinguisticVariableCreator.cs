using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations
{
    public class LinguisticVariableCreator : ILinguisticVariableCreator
    {
        private readonly IMembershipFunctionCreator _membershipFunctionCreator;
        private readonly ILinguisticVariableParser _linguisticVariableParser;

        public LinguisticVariableCreator(
            IMembershipFunctionCreator membershipFunctionCreator,
            ILinguisticVariableParser linguisticVariableParser)
        {
            _membershipFunctionCreator = membershipFunctionCreator ?? throw new ArgumentNullException(nameof(membershipFunctionCreator));
            _linguisticVariableParser = linguisticVariableParser ?? throw new ArgumentNullException(nameof(linguisticVariableParser));
        }

        public List<LinguisticVariable> CreateLinguisticVariableEntities(string linguisticVariable)
        {
            var linguisticVariableStringsList = _linguisticVariableParser.ParseLinguisticVariable(linguisticVariable);
            return linguisticVariableStringsList.Select(CreateLinguisticVariableEntity).ToList();
        }

        private LinguisticVariable CreateLinguisticVariableEntity(LinguisticVariableStrings linguisticVariableStrings)
        {
            var isInitial = linguisticVariableStrings.DataOrigin.ToEnum<DataOriginType>() == DataOriginType.Initial;
            var membershipFunctions = new MembershipFunctionList();
            foreach (var membershipFunctionStrings in linguisticVariableStrings.MembershipFunctions)
            {
                var functionType = membershipFunctionStrings.MembershipFunctionType.ToEnum<MembershipFunctionType>();
                var membershipFunction = _membershipFunctionCreator.CreateMembershipFunctionEntity(
                    functionType, membershipFunctionStrings.MembershipFunctionName, membershipFunctionStrings.MembershipFunctionValues);
                membershipFunctions.Add(membershipFunction);
            }
            return new LinguisticVariable(linguisticVariableStrings.VariableName, membershipFunctions, isInitial);
        }
    }
}