using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Interfaces;
using DataProvider.Entities;
using DataProvider.Interfaces;

namespace DataProvider.Implementations
{
    public class CsvDataProvider : IDataProvider
    {
        private readonly IFileParser<List<string[]>> _csvParser;
        private readonly IParsingResultValidator _validator;
        private readonly IDataFilePathProvider _filePathProvider;
        private readonly IValidationOperationResultLogger _validationOperationResultLoger;

        public CsvDataProvider(
            IFileParser<List<string[]>> csvParser,
            IParsingResultValidator validator,
            IDataFilePathProvider filePathProvider,
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

        public Optional<List<InitialData>> GetInitialData()
        {
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            string initialDataFilePath = _filePathProvider.FilePath;
            List<string[]> parsingResult = _csvParser.ParseFile(initialDataFilePath);
            ValidationOperationResult operationResult = _validator.Validate(parsingResult);

            if (operationResult.IsSuccess)
            {
                List<InitialData> initialData = parsingResult.Select(
                    strings => new InitialData(
                        strings[0],
                        double.Parse(strings[1], CultureInfo.InvariantCulture),
                        double.Parse(strings[2], CultureInfo.InvariantCulture)))
                    .ToList();
                return Optional<List<InitialData>>.For(initialData);
            }

            _validationOperationResultLoger.LogValidationOperationResultMessages(operationResult);
            return Optional<List<InitialData>>.Empty();
        }
    }
}