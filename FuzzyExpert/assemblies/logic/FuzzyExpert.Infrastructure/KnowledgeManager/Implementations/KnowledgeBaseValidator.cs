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
            var allVariableNames = linguisticVariables.Select(lv => lv.VariableName).ToList();

            var ifStatementsLinguisticVariableNames = implicationRules
                .SelectMany(ir => ir.IfStatement.SelectMany(ifs => ifs.UnaryStatements.Select(us => us.LeftOperand)))
                .ToList();
            var thenStatementsLinguisticVariableNames = implicationRules
                .SelectMany(ir => ir.ThenStatement.UnaryStatements.Select(us => us.LeftOperand))
                .ToList();
            var implicationRulesLinguisticVariableNames = new List<string>(ifStatementsLinguisticVariableNames.Concat(thenStatementsLinguisticVariableNames));

            var validationMessages = implicationRulesLinguisticVariableNames
                .Where(implicationRulesLinguisticVariableName => !allVariableNames.Contains(implicationRulesLinguisticVariableName))
                .Select(implicationRulesLinguisticVariableName => $"Knowledge base: linguistic variable {implicationRulesLinguisticVariableName} is unknown to linguistic variable base")
                .ToList();

            return validationMessages.Any() ? ValidationOperationResult.Fail(validationMessages) : ValidationOperationResult.Success();
        }
    }
}