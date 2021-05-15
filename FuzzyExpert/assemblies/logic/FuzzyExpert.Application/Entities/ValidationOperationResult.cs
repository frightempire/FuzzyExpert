using System.Collections.Generic;

namespace FuzzyExpert.Application.Entities
{
    public class ValidationOperationResult
    {
        public List<string> Messages { get; }

        public bool Successful { get; }

        public bool Failed => !Successful;

        private ValidationOperationResult(bool successful, List<string> errorMessages)
        {
            Messages = errorMessages;
            Successful = successful;
        }

        public static ValidationOperationResult Success() => new ValidationOperationResult(true, null);

        public static ValidationOperationResult Fail(List<string> errorMessages) => new ValidationOperationResult(false, errorMessages);
    }
}