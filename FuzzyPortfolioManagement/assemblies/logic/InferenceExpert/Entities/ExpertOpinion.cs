using System.Collections.Generic;

namespace InferenceExpert.Entities
{
    public class ExpertOpinion
    {
        public List<string> ErrorMessages { get; }

        public List<string> Result { get; }

        public bool IsSuccess { get; private set; }

        public ExpertOpinion()
        {
            ErrorMessages = new List<string>();
            Result = new List<string>();
            IsSuccess = true;
        }

        public void AddErrorMessage(string message)
        {
            ErrorMessages.Add(message);
            IsSuccess = false;
        }

        public void AddErrorMessages(List<string> messages)
        {
            ErrorMessages.AddRange(messages);
            IsSuccess = false;
        }

        public void AddResult(string result)
        {
            Result.Add(result);
        }

        public void AddResults(List<string> results)
        {
            Result.AddRange(results);
        }
    }
}
