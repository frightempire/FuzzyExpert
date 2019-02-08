using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Interfaces;

namespace KnowledgeManager.Implementations
{
    public class FileLinguisticVariableProvider : ILinguisticVariableProvider
    {
        private readonly IFilePathProvider _filePathProvider;
        private readonly IFileReader _fileReader;
        private readonly ILinguisticVariableValidator _linguisticVariableValidator;
        private readonly ILinguisticVariableParser _linguisticVariableParser;
        private readonly ILinguisticVariableCreator _linguisticVariableCreator;

        public FileLinguisticVariableProvider(
            ILinguisticVariableValidator linguisticVariableValidator,
            ILinguisticVariableParser linguisticVariableParser,
            ILinguisticVariableCreator linguisticVariableCreator,
            IFilePathProvider filePathProvider,
            IFileReader fileReader)
        {
            ExceptionAssert.IsNull(linguisticVariableValidator);
            ExceptionAssert.IsNull(linguisticVariableParser);
            ExceptionAssert.IsNull(linguisticVariableCreator);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(fileReader);

            _linguisticVariableValidator = linguisticVariableValidator;
            _linguisticVariableParser = linguisticVariableParser;
            _linguisticVariableCreator = linguisticVariableCreator;
            _filePathProvider = filePathProvider;
            _fileReader = fileReader;
        }

        public List<LinguisticVariable> GetLinguisticVariables()
        {
            ExceptionAssert.IsNull(_filePathProvider.FilePath);
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            string linguisticVariablesFilePath = _filePathProvider.FilePath;
            List<string> linguisticVariablesFromFile = _fileReader.ReadFileByLines(linguisticVariablesFilePath);

            List<LinguisticVariable> linguisticVariables = new List<LinguisticVariable>();
            linguisticVariablesFromFile.ForEach(lvff =>
            {
                _linguisticVariableValidator.ValidateLinguisticVariable(lvff);
                LinguisticVariableStrings linguisticVariableStrings = _linguisticVariableParser.ParseLinguisticVariable(lvff);
                LinguisticVariable linguisticVariable = _linguisticVariableCreator.CreateLinguisticVariableEntity(linguisticVariableStrings);
                linguisticVariables.Add(linguisticVariable);
            });

            return linguisticVariables;
        }
    }
}