using System.Collections.Generic;
using System.Linq;
using CommonLogic.Entities;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class KnowledgeBaseValidator : IKnowledgeBaseValidator
    {
        public ValidationOperationResult ValidateLinguisticVariablesNames(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables)
        {
            ValidationOperationResult validationOperationResult = new ValidationOperationResult();

            List<string> initialVariableNames = linguisticVariables
                .Where(lv => lv.Value.IsInitialData)
                .Select(lv => lv.Value.VariableName)
                .ToList();
            List<string> derivativeVariableNames = linguisticVariables
                .Where(lv => !lv.Value.IsInitialData)
                .Select(lv => lv.Value.VariableName)
                .ToList();
            List<string> allVariableNames = new List<string>();
            allVariableNames.AddRange(initialVariableNames);
            allVariableNames.AddRange(derivativeVariableNames);

            List<string> ifStatementsLinguisticVariableNames = implicationRules
                .SelectMany(ir => ir.Value.IfStatement.SelectMany(ifs => ifs.UnaryStatements.Select(us => us.LeftOperand)))
                .ToList();
            List<string> thenStatementsLinguisticVariableNames = implicationRules
                .SelectMany(ir => ir.Value.ThenStatement.UnaryStatements.Select(us => us.LeftOperand))
                .ToList();
            List<string> implicationRulesLinguisticVariableNames = new List<string>();
            implicationRulesLinguisticVariableNames.AddRange(ifStatementsLinguisticVariableNames);
            implicationRulesLinguisticVariableNames.AddRange(thenStatementsLinguisticVariableNames);

            foreach (string implicationRulesLinguisticVariableName in implicationRulesLinguisticVariableNames)
            {
                if (!allVariableNames.Contains(implicationRulesLinguisticVariableName))
                    validationOperationResult.AddMessage(
                        $"Knowledge base: linguistic variable {implicationRulesLinguisticVariableName} is unknown to linguistic variable base");
            }

            return validationOperationResult;
        }
    }
}