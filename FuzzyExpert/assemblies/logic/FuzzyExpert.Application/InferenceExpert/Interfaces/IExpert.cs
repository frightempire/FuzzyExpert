using FuzzyExpert.Application.InferenceExpert.Entities;

namespace FuzzyExpert.Application.InferenceExpert.Interfaces
{
    public interface IExpert
    {
        ExpertOpinion GetResult(string profileName);
    }
}