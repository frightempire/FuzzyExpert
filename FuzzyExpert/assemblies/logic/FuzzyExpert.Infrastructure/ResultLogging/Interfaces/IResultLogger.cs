using System.Collections.Generic;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.ResultLogging.Interfaces
{
    public interface IResultLogger
    {
        string LogInferenceResult(Dictionary<int, ImplicationRule> implicationRules, ExpertOpinion expertOpinion, string userName);
    }
}