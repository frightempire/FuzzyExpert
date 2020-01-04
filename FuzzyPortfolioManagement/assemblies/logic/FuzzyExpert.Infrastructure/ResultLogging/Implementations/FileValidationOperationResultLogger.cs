using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.ResultLogging.Implementations
{
    public class FileValidationOperationResultLogger : IValidationOperationResultLogger
    {
        private readonly IFileOperations _fileOperations;

        public FileValidationOperationResultLogger(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
        }

        public void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult, int errorLine)
        {
            string errorLineString = $"Line {errorLine}";
            List<string> errorMessages = validationOperationResult.Messages;
            List<string> errorMessagesWithLines = errorMessages.AppendToEachString(errorLineString);

            string separatorHeader = DateTime.Now.ToLongTimeString();
            errorMessagesWithLines.Insert(0, separatorHeader);

            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + @"\ValidationLog.txt";
            _fileOperations.AppendLinesToFile(pathToFile, errorMessagesWithLines);
        }

        public void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult)
        {
            List<string> errorMessages = validationOperationResult.Messages;

            string separatorHeader = DateTime.Now.ToLongTimeString();
            errorMessages.Insert(0, separatorHeader);

            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + @"\ValidationLog.txt";
            _fileOperations.AppendLinesToFile(pathToFile, errorMessages);
        }
    }
}