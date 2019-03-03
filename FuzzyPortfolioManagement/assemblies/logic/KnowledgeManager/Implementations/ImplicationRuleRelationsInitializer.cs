using System.Collections.Generic;
using System.Linq;
using KnowledgeManager.Entities;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class ImplicationRuleRelationsInitializer : IImplicationRuleRelationsInitializer
    {
        public List<ImplicationRuleRelations> FormImplicationRuleRelations(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables)
        {
            List<ImplicationRuleRelations> implicationRulesRelations = new List<ImplicationRuleRelations>();
            foreach (KeyValuePair<int, ImplicationRule> implicationRule in implicationRules)
            {
                int currentRuleNumber = implicationRule.Key;
                List<ImplicationRulesConnection> antecedentRuleNumbers = FormAntecedentRuleNumbersForImplicationRule(implicationRule, implicationRules);
                List<ImplicationRulesConnection> decendentRuleNumbers = FormDecendentRuleNumbersForImplicationRule(implicationRule, implicationRules);
                List<int> linguisticVariableNumbers = FormLinguisticVariableNumbersForImplicationRule(linguisticVariables, implicationRule);

                implicationRulesRelations.Add(new ImplicationRuleRelations(
                    currentRuleNumber, antecedentRuleNumbers, decendentRuleNumbers, linguisticVariableNumbers));
            }

            return implicationRulesRelations;
        }

        private List<int> FormLinguisticVariableNumbersForImplicationRule(Dictionary<int, LinguisticVariable> linguisticVariables,
            KeyValuePair<int, ImplicationRule> implicationRule)
        {
            List<int> linguisticVariableNumbers = new List<int>();
            List<UnaryStatement> ifUnaryStatements = implicationRule.Value.IfStatement
                .SelectMany(sc => sc.UnaryStatements).ToList();
            List<UnaryStatement> thenUnaryStatements = implicationRule.Value.ThenStatement.UnaryStatements;
            List<UnaryStatement> allUnaryStatements = new List<UnaryStatement>();
            allUnaryStatements.AddRange(ifUnaryStatements);
            allUnaryStatements.AddRange(thenUnaryStatements);

            foreach (UnaryStatement unaryStatement in allUnaryStatements)
            {
                foreach (KeyValuePair<int, LinguisticVariable> linguisticVariable in linguisticVariables)
                {
                    if (linguisticVariable.Value.VariableName == unaryStatement.LeftOperand &&
                        !linguisticVariableNumbers.Contains(linguisticVariable.Key))
                        linguisticVariableNumbers.Add(linguisticVariable.Key);
                }
            }
            return linguisticVariableNumbers;
        }

        private List<ImplicationRulesConnection> FormDecendentRuleNumbersForImplicationRule(
            KeyValuePair<int, ImplicationRule> implicationRule,
            Dictionary<int, ImplicationRule> implicationRules)
        {
            List<ImplicationRulesConnection> decendentRuleNumbers = new List<ImplicationRulesConnection>();
            List<UnaryStatement> thenUnaryStatements = implicationRule.Value.ThenStatement.UnaryStatements;
            foreach (UnaryStatement thenUnaryStatement in thenUnaryStatements)
            {
                foreach (KeyValuePair<int, ImplicationRule> rule in implicationRules)
                {
                    List<UnaryStatement> ruleIfStatements =
                        rule.Value.IfStatement.SelectMany(sc => sc.UnaryStatements).ToList();
                    foreach (UnaryStatement ruleIfStatement in ruleIfStatements)
                    {
                        if (UnaryStatementsAreEqual(ruleIfStatement, thenUnaryStatement) &&
                            !decendentRuleNumbers.Select(drn => drn.ConnectedRuleNumber).Contains(rule.Key))
                            decendentRuleNumbers.Add(new ImplicationRulesConnection(rule.Key));
                    }
                }
            }
            return decendentRuleNumbers;
        }

        private List<ImplicationRulesConnection> FormAntecedentRuleNumbersForImplicationRule(
            KeyValuePair<int, ImplicationRule> implicationRule,
            Dictionary<int, ImplicationRule> implicationRules)
        {
            List<ImplicationRulesConnection> antecedentRuleNumbers = new List<ImplicationRulesConnection>();
            List<UnaryStatement> ifUnaryStatements =
                implicationRule.Value.IfStatement.SelectMany(sc => sc.UnaryStatements).ToList();
            foreach (UnaryStatement ifUnaryStatement in ifUnaryStatements)
            {
                foreach (KeyValuePair<int, ImplicationRule> rule in implicationRules)
                {
                    List<UnaryStatement> ruleThenStatements = rule.Value.ThenStatement.UnaryStatements;
                    foreach (UnaryStatement ruleThenStatement in ruleThenStatements)
                    {
                        if (UnaryStatementsAreEqual(ruleThenStatement, ifUnaryStatement) &&
                            !antecedentRuleNumbers.Select(arn => arn.ConnectedRuleNumber).Contains(rule.Key))
                            antecedentRuleNumbers.Add(new ImplicationRulesConnection(rule.Key));
                    }
                }
            }
            return antecedentRuleNumbers;
        }

        private bool UnaryStatementsAreEqual(UnaryStatement ruleThenStatement, UnaryStatement ifUnaryStatement)
        {
            return ruleThenStatement.LeftOperand == ifUnaryStatement.LeftOperand &&
                   ruleThenStatement.RightOperand == ifUnaryStatement.RightOperand &&
                   ruleThenStatement.ComparisonOperation == ifUnaryStatement.ComparisonOperation;
        }
    }
}