using System.Collections.Generic;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces
{
    public interface IKnowledgeBaseValidator
    {
        ValidationOperationResult ValidateLinguisticVariablesNames(
            List<ImplicationRule> implicationRules,
            List<LinguisticVariable> linguisticVariables);
    }
}