using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class FileImplicationRuleProvider : IImplicationRuleProvider
    {
        private readonly IFileOperations _fileOperations;
        private readonly IImplicationRuleFilePathProvider _filePathProvider;
        private readonly IImplicationRuleCreator _implicationRuleCreator;
        private readonly IImplicationRuleValidator _implicationRuleValidator;
        private readonly INameSupervisor _nameSupervisor;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public FileImplicationRuleProvider(
            IFileOperations fileOperations,
            IImplicationRuleFilePathProvider filePathProvider,
            IImplicationRuleValidator implicationRuleValidator,
            IImplicationRuleCreator implicationRuleCreator,
            INameSupervisor nameSupervisor,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _filePathProvider = filePathProvider ?? throw new ArgumentNullException(nameof(filePathProvider));
            _implicationRuleValidator = implicationRuleValidator ?? throw new ArgumentNullException(nameof(implicationRuleValidator));
            _implicationRuleCreator = implicationRuleCreator ?? throw new ArgumentNullException(nameof(implicationRuleCreator));
            _nameSupervisor = nameSupervisor ?? throw new ArgumentNullException(nameof(nameSupervisor));
            _validationOperationResultLogger = validationOperationResultLogger ?? throw new ArgumentNullException(nameof(validationOperationResultLogger));
        }

        public Optional<List<ImplicationRule>> GetImplicationRules()
        {
            if (!File.Exists(_filePathProvider.FilePath)) throw new FileNotFoundException(nameof(_filePathProvider.FilePath));

            List<string> implicationRulesFromFile = _fileOperations.ReadFileByLines(_filePathProvider.FilePath);

            List<ImplicationRule> implicationRules = new List<ImplicationRule>();
            for (var i = 0; i < implicationRulesFromFile.Count; i++)
            {
                var implicationRuleFromFile = implicationRulesFromFile[i];
                string preProcessedImplicationRule = implicationRuleFromFile.RemoveUnwantedCharacters(new List<char> {' '});
                ValidationOperationResult validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(preProcessedImplicationRule);
                if (validationOperationResult.IsSuccess)
                {
                    ImplicationRule implicationRule = _implicationRuleCreator.CreateImplicationRuleEntity(preProcessedImplicationRule);
                    implicationRules.Add(implicationRule);
                }
                else
                {
                    int line = i + 1;
                    _validationOperationResultLogger.LogValidationOperationResultMessages(validationOperationResult, line);
                }
            }

            if (implicationRules.Count == 0) return Optional<List<ImplicationRule>>.Empty();

            List<ImplicationRule> separatedImplicationRules = DivideComplexImplicationRules(implicationRules);
            SetNamesForUnatyStatements(separatedImplicationRules);
            return Optional<List<ImplicationRule>>.For(separatedImplicationRules);
        }

        private List<ImplicationRule> DivideComplexImplicationRules(List<ImplicationRule> implicationRules)
        {
            List<ImplicationRule> grownRuleList = new List<ImplicationRule>();

            foreach (var implicationRule in implicationRules)
            {
                if (implicationRule.IfStatement.Count > 1)
                {
                    foreach (var statementCombination in implicationRule.IfStatement)
                    {
                        ImplicationRule dividedImplicationRule =
                            new ImplicationRule(new List<StatementCombination> {statementCombination},
                                implicationRule.ThenStatement);
                        grownRuleList.Add(dividedImplicationRule);
                    }
                }
                else
                {
                    grownRuleList.Add(implicationRule);
                }
            }

            return grownRuleList;
        }

        private void SetNamesForUnatyStatements(List<ImplicationRule> implicationRules)
        {
            List<UnaryStatement> ifUnaryStatements = implicationRules.SelectMany(ir => ir.IfStatement.SelectMany(ifs => ifs.UnaryStatements)).ToList();
            List<UnaryStatement> thenUnaryStatements = implicationRules.SelectMany(ir => ir.ThenStatement.UnaryStatements).ToList();
            List<UnaryStatement> allUnaryStatements = new List<UnaryStatement>();
            allUnaryStatements.AddRange(ifUnaryStatements);
            allUnaryStatements.AddRange(thenUnaryStatements);
            _nameSupervisor.AssignNames(allUnaryStatements);
        }
    }
}