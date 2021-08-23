using System;
using System.Collections.Generic;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Application.InferenceExpert.Entities
{
    public class ExpertOpinion
    {
        public List<string> ErrorMessages { get; }

        public List<DeFuzzifiedInferenceResult> Result { get; }

        public bool IsSuccess { get; private set; }

        public ExpertOpinion()
        {
            ErrorMessages = new List<string>();
            Result = new List<DeFuzzifiedInferenceResult>();
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

        public void AddResults(List<DeFuzzifiedInferenceResult> results)
        {
            Result.AddRange(results);
        }
    }
}