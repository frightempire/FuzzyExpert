using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces
{
    public interface IImplicationRuleProvider
    {
        Optional<List<ImplicationRule>> GetImplicationRules(string profileName);
    }
}