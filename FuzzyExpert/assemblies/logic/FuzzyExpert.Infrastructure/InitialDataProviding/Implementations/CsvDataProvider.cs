using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.InitialDataProviding.Implementations
{
    public class CsvDataProvider : IDataProvider
    {
        private readonly IFileParser<List<string[]>> _csvParser;
        private readonly IParsingResultValidator _validator;
        private readonly IDataFilePathProvider _filePathProvider;
        private readonly IValidationOperationResultLogger _validationOperationResultLogger;

        public CsvDataProvider(
            IFileParser<List<string[]>> csvParser,
            IParsingResultValidator validator,
            IDataFilePathProvider filePathProvider,
            IValidationOperationResultLogger validationOperationResultLogger)
        {
            _csvParser = csvParser ?? throw new ArgumentNullException(nameof(csvParser));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _filePathProvider = filePathProvider ?? throw new ArgumentNullException(nameof(filePathProvider));
            _validationOperationResultLogger = validationOperationResultLogger ?? throw new ArgumentNullException(nameof(validationOperationResultLogger));
        }

        public Optional<List<InitialData>> GetInitialData()
        {
            if (!File.Exists(_filePathProvider.FilePath)) throw new FileNotFoundException(nameof(_filePathProvider.FilePath));

            string initialDataFilePath = _filePathProvider.FilePath;
            List<string[]> parsingResult = _csvParser.ParseFile(initialDataFilePath);
            ValidationOperationResult operationResult = _validator.Validate(parsingResult);

            if (operationResult.Successful)
            {
                List<InitialData> initialData = parsingResult.Select(
                    strings => new InitialData(
                        strings[0],
                        double.Parse(strings[1], CultureInfo.InvariantCulture),
                        double.Parse(strings[2], CultureInfo.InvariantCulture)))
                    .ToList();
                return Optional<List<InitialData>>.For(initialData);
            }

            _validationOperationResultLogger.LogValidationOperationResultMessages(operationResult);
            return Optional<List<InitialData>>.Empty();
        }
    }
}