using System.Collections.Generic;

namespace InferenceExpert.Entities
{
    public class ExpertOpinion
    {
        public List<string> ErrorMessages { get; }

        public Dictionary<string, double> Result { get; }

        public bool IsSuccess { get; private set; }

        public ExpertOpinion()
        {
            ErrorMessages = new List<string>();
            Result = new Dictionary<string, double>();
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

        public void AddResult(KeyValuePair<string, double> result)
        {
            Result.Add(result.Key, result.Value);
        }

        public void AddResults(Dictionary<string, double> results)
        {
            foreach (var result in results)
            {
                Result.Add(result.Key, result.Value);
            }
        }
    }
}
