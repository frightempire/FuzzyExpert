using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class FileLinguisticVariableProvider : ILinguisticVariableProvider
    {
        private readonly ILinguisticVariableFilePathProvider _filePathProvider;
        private readonly IFileOperations _fileOperations;
        private readonly ILinguisticVariableValidator _linguisticVariableValidator;
        private readonly ILinguisticVariableParser _linguisticVariableParser;
        private readonly ILinguisticVariableCreator _linguisticVariableCreator;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public FileLinguisticVariableProvider(
            ILinguisticVariableValidator linguisticVariableValidator,
            ILinguisticVariableParser linguisticVariableParser,
            ILinguisticVariableCreator linguisticVariableCreator,
            ILinguisticVariableFilePathProvider filePathProvider,
            IFileOperations fileOperations,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            _linguisticVariableValidator = linguisticVariableValidator ?? throw new ArgumentNullException(nameof(linguisticVariableValidator));
            _linguisticVariableParser = linguisticVariableParser ?? throw new ArgumentNullException(nameof(linguisticVariableValidator));
            _linguisticVariableCreator = linguisticVariableCreator ?? throw new ArgumentNullException(nameof(linguisticVariableValidator));
            _filePathProvider = filePathProvider ?? throw new ArgumentNullException(nameof(linguisticVariableValidator));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(linguisticVariableValidator));
            _validationOperationResultLogger = validationOperationResultLogger ?? throw new ArgumentNullException(nameof(linguisticVariableValidator));
        }

        public Optional<List<LinguisticVariable>> GetLinguisticVariables()
        {
            if (!File.Exists(_filePathProvider.FilePath)) throw new FileNotFoundException(_filePathProvider.FilePath);

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