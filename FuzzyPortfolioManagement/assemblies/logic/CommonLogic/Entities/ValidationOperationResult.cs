using System.Collections.Generic;

namespace CommonLogic.Entities
{
    public class ValidationOperationResult
    {
        private List<string> Messages { get; }

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

        public List<string> GetMessages() => Messages;
    }
}
