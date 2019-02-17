using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Extensions;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Interfaces;

namespace KnowledgeManager.Implementations
{
    public class FileImplicationRuleProvider : IImplicationRuleProvider
    {
        private readonly IFileReader _fileReader;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IImplicationRuleCreator _implicationRuleCreator;
        private readonly IImplicationRuleParser _implicationRuleParser;
        private readonly IImplicationRuleValidator _implicationRuleValidator;

        public FileImplicationRuleProvider(
            IFileReader fileReader,
            IFilePathProvider filePathProvider,
            IImplicationRuleValidator implicationRuleValidator,
            IImplicationRuleParser implicationRuleParser,
            IImplicationRuleCreator implicationRuleCreator)
        {
            ExceptionAssert.IsNull(fileReader);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(implicationRuleValidator);
            ExceptionAssert.IsNull(implicationRuleParser);
            ExceptionAssert.IsNull(implicationRuleCreator);

            _fileReader = fileReader;
            _filePathProvider = filePathProvider;
            _implicationRuleValidator = implicationRuleValidator;
            _implicationRuleParser = implicationRuleParser;
            _implicationRuleCreator = implicationRuleCreator;
        }

        public List<ImplicationRule> GetImplicationRules()
        {
            ExceptionAssert.IsNull(_filePathProvider.FilePath);
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            List<string> implicationRulesFromFile = _fileReader.ReadFileByLines(_filePathProvider.FilePath);

            List<ImplicationRule> implicationRules = new List<ImplicationRule>();
            for (var i = 0; i < implicationRulesFromFile.Count; i++)
            {
                var implicationRuleFromFile = implicationRulesFromFile[i];
                string preProcessedImplicationRule = implicationRuleFromFile.RemoveUnwantedCharacters(new List<char> {' '});
                ValidationOperationResult validationOperationResult =
                    _implicationRuleValidator.ValidateImplicationRule(preProcessedImplicationRule);
                if (validationOperationResult.IsSuccess)
                {
                    ImplicationRuleStrings implicationRuleStrings =
                        _implicationRuleParser.ExtractStatementParts(preProcessedImplicationRule);
                    ImplicationRule implicationRule =
                        _implicationRuleCreator.CreateImplicationRuleEntity(implicationRuleStrings);
                    implicationRules.Add(implicationRule);
                }
                else
                {
                    int line = i + 1;
                    // log line and error messages
                }
            }

            return implicationRules;
        }
    }
}