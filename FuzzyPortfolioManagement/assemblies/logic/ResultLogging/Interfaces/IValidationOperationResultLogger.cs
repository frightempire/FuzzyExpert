using CommonLogic.Entities;

namespace ResultLogging.Interfaces
{
    public interface IValidationOperationResultLogger
    {
        void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult, int errorLine);

        void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult);
    }
}
