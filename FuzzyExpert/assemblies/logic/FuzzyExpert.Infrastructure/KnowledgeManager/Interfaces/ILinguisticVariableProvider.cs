using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableProvider
    {
        Optional<List<LinguisticVariable>> GetLinguisticVariables(string profileName);
    }
}