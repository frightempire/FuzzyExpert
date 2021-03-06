﻿using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Entities;

namespace FuzzyExpert.Base.UnitTests
{
    public static class ObjectComparer
    {
        public static bool ImplicationRulesAreEqual(
            ImplicationRule implicationRuleToCompare,
            ImplicationRule implicationRuleToCompareWith)
        {
            if (implicationRuleToCompare.IfStatement.Count != implicationRuleToCompareWith.IfStatement.Count)
                return false;

            for (int i = 0; i < implicationRuleToCompare.IfStatement.Count; i++)
            {
                List<UnaryStatement> ifUnaryStatementsToCompare = implicationRuleToCompare.IfStatement[i].UnaryStatements;
                List<UnaryStatement> ifUnaryStatementsToCompareWith = implicationRuleToCompareWith.IfStatement[i].UnaryStatements;

                if (ifUnaryStatementsToCompare.Count != ifUnaryStatementsToCompareWith.Count)
                    return false;

                for (var j = 0; j < ifUnaryStatementsToCompare.Count; j++)
                {
                    if (!UnaryStatementsAreEqual(ifUnaryStatementsToCompare[j], ifUnaryStatementsToCompareWith[j]))
                        return false;
                }
            }

            List<UnaryStatement> thenUnaryStatementsToCompare = implicationRuleToCompare.ThenStatement.UnaryStatements;
            List<UnaryStatement> thenUnaryStatementsToCompareWith = implicationRuleToCompareWith.ThenStatement.UnaryStatements;

            if (thenUnaryStatementsToCompare.Count != thenUnaryStatementsToCompareWith.Count)
                return false;

            for (var i = 0; i < thenUnaryStatementsToCompare.Count; i++)
            {
                if (!UnaryStatementsAreEqual(thenUnaryStatementsToCompare[i], thenUnaryStatementsToCompareWith[i]))
                    return false;
            }

            return true;
        }

        public static bool ImplicationRuleStringsAreEqual(
            ImplicationRuleStrings implicationRuleStringsToCompare,
            ImplicationRuleStrings implicationRuleStringsToCompareWith)
        {
            return implicationRuleStringsToCompare.IfStatement == implicationRuleStringsToCompareWith.IfStatement &&
                   implicationRuleStringsToCompare.ThenStatement == implicationRuleStringsToCompareWith.ThenStatement;
        }

        public static bool UnaryStatementsAreEqual(
            UnaryStatement unaryStatementToCompare,
            UnaryStatement unaryStatementToCompareWith)
        {
            return unaryStatementToCompare.LeftOperand == unaryStatementToCompareWith.LeftOperand &&
                   unaryStatementToCompare.ComparisonOperation == unaryStatementToCompareWith.ComparisonOperation &&
                   unaryStatementToCompare.RightOperand == unaryStatementToCompareWith.RightOperand;
        }

        public static bool MembershipFunctionsAreEqual(
            MembershipFunction membershipFunctionToCompare,
            MembershipFunction membershipFunctionToCompareWith)
        {
            Type membershipFunctionToCompareType = membershipFunctionToCompare.GetType();
            Type membershipFunctionToCompareWithType = membershipFunctionToCompareWith.GetType();

            return membershipFunctionToCompareType == membershipFunctionToCompareWithType &&
                   membershipFunctionToCompare.Equals(membershipFunctionToCompareWith);
        }

        public static bool MembershipFunctionListsAreEqual(
            MembershipFunctionList membershipFunctionListToCompare,
            MembershipFunctionList membershipFunctionListToCompareWith)
        {
            if (membershipFunctionListToCompare.Count != membershipFunctionListToCompareWith.Count)
                return false;

            for (int i = 0; i < membershipFunctionListToCompare.Count; i++)
            {
                if (!MembershipFunctionsAreEqual(membershipFunctionListToCompare[i], membershipFunctionListToCompareWith[i]))
                    return false;
            }

            return true;
        }

        public static bool MembershipFunctionStringsAreEqual(
            MembershipFunctionStrings membershipFunctionStringsToCompare,
            MembershipFunctionStrings membershipFunctionStringsToCompareWith)
        {
            if (membershipFunctionStringsToCompare.MembershipFunctionValues.Count !=
                membershipFunctionStringsToCompareWith.MembershipFunctionValues.Count)
                return false;

            for (int i = 0; i < membershipFunctionStringsToCompare.MembershipFunctionValues.Count; i++)
            {
                if (membershipFunctionStringsToCompare.MembershipFunctionValues[i] !=
                    membershipFunctionStringsToCompareWith.MembershipFunctionValues[i])
                    return false;
            }

            return membershipFunctionStringsToCompare.MembershipFunctionName == membershipFunctionStringsToCompareWith.MembershipFunctionName &&
                   membershipFunctionStringsToCompare.MembershipFunctionType == membershipFunctionStringsToCompareWith.MembershipFunctionType;
        }

        public static bool LinguisticVariableStringsAreEqual(
            LinguisticVariableStrings linguisticVariableStringsToCompare,
            LinguisticVariableStrings linguisticVariableStringsToCompareWith)
        {
            if (linguisticVariableStringsToCompare.MembershipFunctions.Count != linguisticVariableStringsToCompareWith.MembershipFunctions.Count)
                return false;

            for (int i = 0; i < linguisticVariableStringsToCompare.MembershipFunctions.Count; i++)
            {
                if (!MembershipFunctionStringsAreEqual(
                    linguisticVariableStringsToCompare.MembershipFunctions[i],
                    linguisticVariableStringsToCompareWith.MembershipFunctions[i]))
                    return false;
            }

            return linguisticVariableStringsToCompare.VariableName == linguisticVariableStringsToCompareWith.VariableName &&
                   linguisticVariableStringsToCompare.DataOrigin == linguisticVariableStringsToCompareWith.DataOrigin;
        }

        public static bool LinguisticVariablesAreEqual(
            LinguisticVariable linguisticVariableToCompare,
            LinguisticVariable linguisticVariableToCompareWith)
        {
            if (linguisticVariableToCompare.MembershipFunctionList.Count != linguisticVariableToCompareWith.MembershipFunctionList.Count)
                return false;

            for (int i = 0; i < linguisticVariableToCompare.MembershipFunctionList.Count; i++)
            {
                if (!MembershipFunctionsAreEqual(
                    linguisticVariableToCompare.MembershipFunctionList[i],
                    linguisticVariableToCompareWith.MembershipFunctionList[i]))
                    return false;
            }

            return linguisticVariableToCompare.VariableName == linguisticVariableToCompareWith.VariableName &&
                   linguisticVariableToCompare.IsInitialData == linguisticVariableToCompareWith.IsInitialData;
        }

        public static bool LinguisticVariableRelationsAreEqual(
            LinguisticVariableRelations linguisticVariableRelationsToCompare,
            LinguisticVariableRelations linguisticVariableRelationsToCompareWith)
        {
            if (linguisticVariableRelationsToCompare.LinguisticVariableNumber !=
                linguisticVariableRelationsToCompareWith.LinguisticVariableNumber)
                return false;

            if (linguisticVariableRelationsToCompare.RelatedUnaryStatementNames.Count !=
                linguisticVariableRelationsToCompareWith.RelatedUnaryStatementNames.Count)
                return false;

            for (int i = 0; i < linguisticVariableRelationsToCompare.RelatedUnaryStatementNames.Count; i++)
            {
                if (linguisticVariableRelationsToCompare.RelatedUnaryStatementNames[i] !=
                    linguisticVariableRelationsToCompareWith.RelatedUnaryStatementNames[i])
                    return false;
            }

            return true;
        }

        public static bool InitialDatasAreEqual(
            InitialData initialDataToCompare,
            InitialData initialDataToCompareWith)
        {
            return initialDataToCompare.Name == initialDataToCompareWith.Name &&
                   initialDataToCompare.Value == initialDataToCompareWith.Value &&
                   initialDataToCompare.ConfidenceFactor == initialDataToCompareWith.ConfidenceFactor;
        }
    }
}