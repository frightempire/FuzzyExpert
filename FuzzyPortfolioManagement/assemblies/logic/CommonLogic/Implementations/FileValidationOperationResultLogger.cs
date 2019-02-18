using System;
using System.Collections.Generic;
using CommonLogic.Entities;
using CommonLogic.Extensions;
using CommonLogic.Interfaces;

namespace CommonLogic.Implementations
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
            List<string> errorMessages = validationOperationResult.GetMessages();
            List<string> errorMessagesWithLines = errorMessages.AppendToEachString(errorLineString);

            string separatorHeader = DateTime.Now.ToLongTimeString();
            errorMessagesWithLines.Insert(0, separatorHeader);

            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog.txt";
            _fileOperations.AppendLinesToFile(pathToFile, errorMessagesWithLines);
        }
    }
}