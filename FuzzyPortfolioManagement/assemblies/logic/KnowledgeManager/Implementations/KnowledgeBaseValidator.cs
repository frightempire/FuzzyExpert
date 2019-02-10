using System;
using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class KnowledgeBaseValidator : IKnowledgeBaseValidator
    {
        private readonly IImplicationRuleManager _implicationRuleManager;
        private readonly ILinguisticVariableManager _linguisticVariableManager;

        public KnowledgeBaseValidator(IImplicationRuleManager implicationRuleManager, ILinguisticVariableManager linguisticVariableManager)
        {
            ExceptionAssert.IsNull(implicationRuleManager);
            ExceptionAssert.IsNull(linguisticVariableManager);
            _implicationRuleManager = implicationRuleManager;
            _linguisticVariableManager = linguisticVariableManager;
        }

        public void ValidateLinguisticVariablesNames()
        {
            List<ImplicationRule> implicationRules = _implicationRuleManager.ImplicationRules;
            List<LinguisticVariable> linguisticVariables = _linguisticVariableManager.LinguisticVariables;

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
                    throw new ArgumentException(
                        "Knowledge base: one of linguistic variables in implication rule is unknown to linguistic variable base");
            }
        }
    }
}