using FuzzyExpert.Application.Entities;

namespace FuzzyExpert.Infrastructure.ResultLogging.Interfaces
{
    public interface IValidationOperationResultLogger
    {
        void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult, int errorLine);

        void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult);
    }
}