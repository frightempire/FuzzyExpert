using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Extensions;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Interfaces;

namespace KnowledgeManager.Implementations
{
    public class FileLinguisticVariableProvider : ILinguisticVariableProvider
    {
        private readonly IFilePathProvider _filePathProvider;
        private readonly IFileOperations _fileOperations;
        private readonly ILinguisticVariableValidator _linguisticVariableValidator;
        private readonly ILinguisticVariableParser _linguisticVariableParser;
        private readonly ILinguisticVariableCreator _linguisticVariableCreator;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public FileLinguisticVariableProvider(
            ILinguisticVariableValidator linguisticVariableValidator,
            ILinguisticVariableParser linguisticVariableParser,
            ILinguisticVariableCreator linguisticVariableCreator,
            IFilePathProvider filePathProvider,
            IFileOperations fileOperations,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            ExceptionAssert.IsNull(linguisticVariableValidator);
            ExceptionAssert.IsNull(linguisticVariableParser);
            ExceptionAssert.IsNull(linguisticVariableCreator);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(fileOperations);
            ExceptionAssert.IsNull(validationOperationResultLogger);

            _linguisticVariableValidator = linguisticVariableValidator;
            _linguisticVariableParser = linguisticVariableParser;
            _linguisticVariableCreator = linguisticVariableCreator;
            _filePathProvider = filePathProvider;
            _fileOperations = fileOperations;
            _validationOperationResultLogger = validationOperationResultLogger;
        }

        public Optional<List<LinguisticVariable>> GetLinguisticVariables()
        {
            ExceptionAssert.IsNull(_filePathProvider.FilePath);
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            string linguisticVariablesFilePath = _filePathProvider.FilePath;
            List<string> linguisticVariablesFromFile = _fileOperations.ReadFileByLines(linguisticVariablesFilePath);

            List<LinguisticVariable> linguisticVariables = new List<LinguisticVariable>();
            for (var i = 0; i < linguisticVariablesFromFile.Count; i++)
            {
                string linguisticVariableFromFile = linguisticVariablesFromFile[i];
                string preProcessedLinguisticVariable = linguisticVariableFromFile.RemoveUnwantedCharacters(new List<char> { ' ' });
                ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(preProcessedLinguisticVariable);
                if (validationOperationResult.IsSuccess)
                {
                    LinguisticVariableStrings linguisticVariableStrings = _linguisticVariableParser.ParseLinguisticVariable(preProcessedLinguisticVariable);
                    LinguisticVariable linguisticVariable = _linguisticVariableCreator.CreateLinguisticVariableEntity(linguisticVariableStrings);
                    linguisticVariables.Add(linguisticVariable);
                }
                else
                {
                    int line = i + 1;
                    _validationOperationResultLogger.LogValidationOperationResultMessages(validationOperationResult, line);
                }
            }

            return linguisticVariables.Count == 0 ?
                Optional<List<LinguisticVariable>>.Empty() :
                Optional<List<LinguisticVariable>>.For(linguisticVariables);
        }
    }
}