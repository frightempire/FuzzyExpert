using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class KnowledgeBaseValidator : IKnowledgeBaseValidator
    {
        public ValidationOperationResult ValidateLinguisticVariablesNames(
            List<ImplicationRule> implicationRules,
            List<LinguisticVariable> linguisticVariables)
        {
            ValidationOperationResult validationOperationResult = new ValidationOperationResult();

            List<string> initialVariableNames = linguisticVariables
                .Where(lv => lv.IsInitialData)
                .Select(lv => lv.VariableName)
                .ToList();
            List<string> derivativeVariableNames = linguisticVariables
                .Where(lv => !lv.IsInitialData)
                .Select(lv => lv.VariableName)
                .ToList();
            List<string> allVariableNames = new List<string>();
            allVariableNames.AddRange(initialVariableNames);
            allVariableNames.AddRange(derivativeVariableNames);

            List<string> ifStatementsLinguisticVariableNames = implicationRules
                .SelectMany(ir => ir.IfStatement.SelectMany(ifs => ifs.UnaryStatements.Select(us => us.LeftOperand)))
                .ToList();
            List<string> thenStatementsLinguisticVariableNames = implicationRules
                .SelectMany(ir => ir.ThenStatement.UnaryStatements.Select(us => us.LeftOperand))
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