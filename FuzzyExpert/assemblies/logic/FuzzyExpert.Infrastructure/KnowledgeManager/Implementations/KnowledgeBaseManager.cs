using System;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
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
            _implicationRuleManager = implicationRuleManager ?? throw new ArgumentNullException(nameof(implicationRuleManager));
            _linguisticVariableManager = linguisticVariableManager ?? throw new ArgumentNullException(nameof(implicationRuleManager));
            _knowledgeBaseValidator = knowledgeBaseValidator ?? throw new ArgumentNullException(nameof(implicationRuleManager));
            _linguisticVariableRelationsInitializer = linguisticVariableRelationsInitializer ?? throw new ArgumentNullException(nameof(implicationRuleManager));
            _validationOperationResultLogger = validationOperationResultLogger ?? throw new ArgumentNullException(nameof(implicationRuleManager));
        }

        public Optional<KnowledgeBase> GetKnowledgeBase(string profileName)
        {
            var implicationRules = _implicationRuleManager.GetImplicationRules(profileName);
            var linguisticVariables = _linguisticVariableManager.GetLinguisticVariables(profileName);

            if (!implicationRules.IsPresent || !linguisticVariables.IsPresent)
            {
                return Optional<KnowledgeBase>.Empty();
            }

            var validationOperationResult = _knowledgeBaseValidator.ValidateLinguisticVariablesNames(
                    implicationRules.Value.Select(ir => ir.Value).ToList(),
                    linguisticVariables.Value.Select(lv => lv.Value).ToList());

            if (validationOperationResult.Successful)
            {
                var linguisticVariablesRelations = _linguisticVariableRelationsInitializer.FormRelations(implicationRules.Value, linguisticVariables.Value);         
                return Optional<KnowledgeBase>.For(new KnowledgeBase(implicationRules.Value, linguisticVariables.Value, linguisticVariablesRelations));
            }

            _validationOperationResultLogger.LogValidationOperationResultMessages(validationOperationResult);
            return Optional<KnowledgeBase>.Empty();
        }
    }
}