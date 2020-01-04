using System.Collections.Generic;

namespace FuzzyExpert.Application.Entities
{
    public class ValidationOperationResult
    {
        public List<string> Messages { get; }

        public bool IsSuccess { get; private set; }

        public ValidationOperationResult()
        {
            Messages = new List<string>();
            IsSuccess = true;
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
            IsSuccess = false;
        }

        public void AddMessages(List<string> messages)
        {
            Messages.AddRange(messages);
            IsSuccess = false;
        }
    }
}
