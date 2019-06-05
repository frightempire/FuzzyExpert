using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Implementations
{
    // Might be rename to work with InferenceEngine
    public class KnowledgeBaseManager : IKnowledgeBaseManager
    {
        private readonly IImplicationRuleManager _implicationRuleManager;
        private readonly ILinguisticVariableManager _linguisticVariableManager;
        private readonly IKnowledgeBaseValidator _knowledgeBaseValidator;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public KnowledgeBaseManager(
            IImplicationRuleManager implicationRuleManager,
            ILinguisticVariableManager linguisticVariableManager,
            IKnowledgeBaseValidator knowledgeBaseValidator,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            ExceptionAssert.IsNull(implicationRuleManager);
            ExceptionAssert.IsNull(linguisticVariableManager);
            ExceptionAssert.IsNull(knowledgeBaseValidator);
            ExceptionAssert.IsNull(validationOperationResultLogger);

            _implicationRuleManager = implicationRuleManager;
            _linguisticVariableManager = linguisticVariableManager;
            _knowledgeBaseValidator = knowledgeBaseValidator;
            _validationOperationResultLogger = validationOperationResultLogger;
        }

        // TODO: Needs work
        // TODO: Dictionaries might not be needed
        public List<string> GetImplicationRulesMap()
        {
            Dictionary<int, ImplicationRule> implicationRules = _implicationRuleManager.ImplicationRules;
            Dictionary<int, LinguisticVariable> linguisticVariables = _linguisticVariableManager.LinguisticVariables;

            ValidationOperationResult validationOperationResult =
                _knowledgeBaseValidator.ValidateLinguisticVariablesNames(implicationRules, linguisticVariables);

            if (validationOperationResult.IsSuccess)
            {
            }
            else
            {
                _validationOperationResultLogger.LogValidationOperationResultMessages(validationOperationResult);
            }

            return new List<string>();
        }
    }
}