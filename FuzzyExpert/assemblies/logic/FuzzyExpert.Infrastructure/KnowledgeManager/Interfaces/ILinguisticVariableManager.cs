using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableManager
    {
        Optional<Dictionary<int, LinguisticVariable>> GetLinguisticVariables(string profileName);
    }
}