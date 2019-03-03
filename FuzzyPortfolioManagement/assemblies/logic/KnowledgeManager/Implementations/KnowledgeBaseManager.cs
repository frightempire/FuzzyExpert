using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Interfaces;
using KnowledgeManager.Entities;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class KnowledgeBaseManager : IKnowledgeBaseManager
    {
        private readonly IImplicationRuleManager _implicationRuleManager;
        private readonly ILinguisticVariableManager _linguisticVariableManager;
        private readonly IKnowledgeBaseValidator _knowledgeBaseValidator;
        private readonly IImplicationRuleRelationsInitializer _implicationRuleRelationsInitializer;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public KnowledgeBaseManager(
            IImplicationRuleManager implicationRuleManager,
            ILinguisticVariableManager linguisticVariableManager,
            IKnowledgeBaseValidator knowledgeBaseValidator,
            IImplicationRuleRelationsInitializer implicationRuleRelationsInitializer,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            ExceptionAssert.IsNull(implicationRuleManager);
            ExceptionAssert.IsNull(linguisticVariableManager);
            ExceptionAssert.IsNull(knowledgeBaseValidator);
            ExceptionAssert.IsNull(implicationRuleRelationsInitializer);
            ExceptionAssert.IsNull(validationOperationResultLogger);

            _implicationRuleManager = implicationRuleManager;
            _linguisticVariableManager = linguisticVariableManager;
            _knowledgeBaseValidator = knowledgeBaseValidator;
            _implicationRuleRelationsInitializer = implicationRuleRelationsInitializer;
            _validationOperationResultLogger = validationOperationResultLogger;
        }

        public List<ImplicationRuleRelations> GetImplicationRulesMap()
        {
            List<ImplicationRuleRelations> implicationRulesRelations = new List<ImplicationRuleRelations>();

            Dictionary<int, ImplicationRule> implicationRules = _implicationRuleManager.ImplicationRules;
            Dictionary<int, LinguisticVariable> linguisticVariables = _linguisticVariableManager.LinguisticVariables;

            ValidationOperationResult validationOperationResult =
                _knowledgeBaseValidator.ValidateLinguisticVariablesNames(implicationRules, linguisticVariables);

            if (validationOperationResult.IsSuccess)
            {
                implicationRulesRelations = _implicationRuleRelationsInitializer.FormImplicationRuleRelations(
                    implicationRules,
                    linguisticVariables);
            }
            else
            {
                _validationOperationResultLogger.LogValidationOperationResultMessages(validationOperationResult);
            }

            return implicationRulesRelations;
        }
    }
}