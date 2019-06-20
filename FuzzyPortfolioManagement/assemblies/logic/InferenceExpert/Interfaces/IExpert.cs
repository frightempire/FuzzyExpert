using System.Collections.Generic;
using InferenceExpert.Entities;

namespace InferenceExpert.Interfaces
{
    public interface IExpert
    {
        ExpertOpinion GetResult();
    }
}