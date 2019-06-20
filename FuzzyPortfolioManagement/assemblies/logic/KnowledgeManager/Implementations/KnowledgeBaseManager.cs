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
        private readonly ILinguisticVariableRelationsInitializer _linguisticVariableRelationsInitializer;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public KnowledgeBaseManager(
            IImplicationRuleManager implicationRuleManager,
            ILinguisticVariableManager linguisticVariableManager,
            IKnowledgeBaseValidator knowledgeBaseValidator,
            ILinguisticVariableRelationsInitializer linguisticVariableRelationsInitializer,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            ExceptionAssert.IsNull(implicationRuleManager);
            ExceptionAssert.IsNull(linguisticVariableManager);
            ExceptionAssert.IsNull(knowledgeBaseValidator);
            ExceptionAssert.IsNull(linguisticVariableRelationsInitializer);
            ExceptionAssert.IsNull(validationOperationResultLogger);

            _implicationRuleManager = implicationRuleManager;
            _linguisticVariableManager = linguisticVariableManager;
            _knowledgeBaseValidator = knowledgeBaseValidator;
            _linguisticVariableRelationsInitializer = linguisticVariableRelationsInitializer;
            _validationOperationResultLogger = validationOperationResultLogger;
        }

        public Optional<KnowledgeBase> GetKnowledgeBase()
        {
            Optional<Dictionary<int, ImplicationRule>> implicationRules = _implicationRuleManager.ImplicationRules;
            Optional<Dictionary<int, LinguisticVariable>> linguisticVariables = _linguisticVariableManager.LinguisticVariables;

            if (!implicationRules.IsPresent || !linguisticVariables.IsPresent) return Optional<KnowledgeBase>.Empty();

            ValidationOperationResult validationOperationResult =
                _knowledgeBaseValidator.ValidateLinguisticVariablesNames(implicationRules.Value, linguisticVariables.Value);

            if (validationOperationResult.IsSuccess)
            {
                List<LinguisticVariableRelations> linguisticVariablesRelations = 
                    _linguisticVariableRelationsInitializer.FormRelations(implicationRules.Value, linguisticVariables.Value);         
                return Optional<KnowledgeBase>.For(new KnowledgeBase(implicationRules.Value, linguisticVariables.Value, linguisticVariablesRelations));
            }

            _validationOperationResultLogger.LogValidationOperationResultMessages(validationOperationResult);
            return Optional<KnowledgeBase>.Empty();
        }
    }
}