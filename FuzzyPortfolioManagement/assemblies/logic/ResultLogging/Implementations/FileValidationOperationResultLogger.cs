using System;
using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Entities;
using CommonLogic.Extensions;
using CommonLogic.Interfaces;
using ResultLogging.Interfaces;

namespace ResultLogging.Implementations
{
    public class FileValidationOperationResultLogger : IValidationOperationResultLogger
    {
        private readonly IFileOperations _fileOperations;

        public FileValidationOperationResultLogger(IFileOperations fileOperations)
        {
            ExceptionAssert.IsNull(fileOperations);
            _fileOperations = fileOperations;
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