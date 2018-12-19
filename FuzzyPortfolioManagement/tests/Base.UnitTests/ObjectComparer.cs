using System;
using System.Collections.Generic;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Implementations;
using ProductionRuleParser.Entities;

namespace Base.UnitTests
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
                List<UnaryStatement> ifUnaryStetementsToCompare = implicationRuleToCompare.IfStatement[i].UnaryStatements;
                List<UnaryStatement> ifUnaryStetementsToCompareWith = implicationRuleToCompareWith.IfStatement[i].UnaryStatements;

                if (ifUnaryStetementsToCompare.Count != ifUnaryStetementsToCompareWith.Count)
                    return false;

                for (var j = 0; j < ifUnaryStetementsToCompare.Count; j++)
                {
                    if (!UnaryStatementsAreEqual(ifUnaryStetementsToCompare[j], ifUnaryStetementsToCompareWith[j]))
                        return false;
                }
            }

            List<UnaryStatement> thenUnaryStetementsToCompare = implicationRuleToCompare.ThenStatement.UnaryStatements;
            List<UnaryStatement> thenUnaryStetementsToCompareWith = implicationRuleToCompareWith.ThenStatement.UnaryStatements;

            if (thenUnaryStetementsToCompare.Count != thenUnaryStetementsToCompareWith.Count)
                return false;

            for (var i = 0; i < thenUnaryStetementsToCompare.Count; i++)
            {
                if (!UnaryStatementsAreEqual(thenUnaryStetementsToCompare[i], thenUnaryStetementsToCompareWith[i]))
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
    }
}
