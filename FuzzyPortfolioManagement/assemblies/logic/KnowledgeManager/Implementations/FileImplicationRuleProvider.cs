using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Extensions;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Interfaces;
using ResultLogging.Interfaces;

namespace KnowledgeManager.Implementations
{
    public class FileImplicationRuleProvider : IImplicationRuleProvider
    {
        private readonly IFileOperations _fileOperations;
        private readonly IImplicationRuleFilePathProvider _filePathProvider;
        private readonly IImplicationRuleCreator _implicationRuleCreator;
        private readonly IImplicationRuleParser _implicationRuleParser;
        private readonly IImplicationRuleValidator _implicationRuleValidator;
        private readonly INameSupervisor _nameSupervisor;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public FileImplicationRuleProvider(
            IFileOperations fileOperations,
            IImplicationRuleFilePathProvider filePathProvider,
            IImplicationRuleValidator implicationRuleValidator,
            IImplicationRuleParser implicationRuleParser,
            IImplicationRuleCreator implicationRuleCreator,
            INameSupervisor nameSupervisor,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            ExceptionAssert.IsNull(fileOperations);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(implicationRuleValidator);
            ExceptionAssert.IsNull(implicationRuleParser);
            ExceptionAssert.IsNull(implicationRuleCreator);
            ExceptionAssert.IsNull(nameSupervisor);
            ExceptionAssert.IsNull(validationOperationResultLogger);

            _fileOperations = fileOperations;
            _filePathProvider = filePathProvider;
            _implicationRuleValidator = implicationRuleValidator;
            _implicationRuleParser = implicationRuleParser;
            _implicationRuleCreator = implicationRuleCreator;
            _nameSupervisor = nameSupervisor;
            _validationOperationResultLogger = validationOperationResultLogger;
        }

        public Optional<List<ImplicationRule>> GetImplicationRules()
        {
            ExceptionAssert.IsNull(_filePathProvider.FilePath);
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            List<string> implicationRulesFromFile = _fileOperations.ReadFileByLines(_filePathProvider.FilePath);

            List<ImplicationRule> implicationRules = new List<ImplicationRule>();
            for (var i = 0; i < implicationRulesFromFile.Count; i++)
            {
                var implicationRuleFromFile = implicationRulesFromFile[i];
                string preProcessedImplicationRule = implicationRuleFromFile.RemoveUnwantedCharacters(new List<char> {' '});
                ValidationOperationResult validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(preProcessedImplicationRule);
                if (validationOperationResult.IsSuccess)
                {
                    ImplicationRuleStrings implicationRuleStrings = _implicationRuleParser.ExtractStatementParts(preProcessedImplicationRule);
                    ImplicationRule implicationRule = _implicationRuleCreator.CreateImplicationRuleEntity(implicationRuleStrings);
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