using CommonLogic.Entities;

namespace CommonLogic.Interfaces
{
    public interface IValidationOperationResultLogger
    {
        void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult, int errorLine);

        void LogValidationOperationResultMessages(ValidationOperationResult validationOperationResult);
    }
}
