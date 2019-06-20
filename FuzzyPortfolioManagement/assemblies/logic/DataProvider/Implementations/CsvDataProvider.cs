using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Interfaces;
using DataProvider.Interfaces;

namespace DataProvider.Implementations
{
    public class CsvDataProvider : IDataProvider
    {
        private readonly IFileParser<List<string[]>> _csvParser;
        private readonly IParsingResultValidator _validator;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IValidationOperationResultLogger _validationOperationResultLoger;

        public CsvDataProvider(
            IFileParser<List<string[]>> csvParser,
            IParsingResultValidator validator,
            IFilePathProvider filePathProvider,
            IValidationOperationResultLogger validationOperationResultLoger)
        {
            ExceptionAssert.IsNull(csvParser);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(validator);
            ExceptionAssert.IsNull(validationOperationResultLoger);

            _csvParser = csvParser;
            _validator = validator;
            _filePathProvider = filePathProvider;
            _validationOperationResultLoger = validationOperationResultLoger;
        }

        public Optional<Dictionary<string, double>> GetInitialData()
        {
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            string initialDataFilePath = _filePathProvider.FilePath;
            List<string[]> parsingResult = _csvParser.ParseFile(initialDataFilePath);
            ValidationOperationResult operationResult = _validator.Validate(parsingResult);

            if (operationResult.IsSuccess)
            {
                Dictionary<string, double> initialData = new Dictionary<string, double>();
                foreach (string[] strings in parsingResult) initialData[strings[0]] = double.Parse(strings[1]);
                return Optional<Dictionary<string, double>>.For(initialData);
            }

            _validationOperationResultLoger.LogValidationOperationResultMessages(operationResult);
            return Optional<Dictionary<string, double>>.Empty();
        }
    }
}