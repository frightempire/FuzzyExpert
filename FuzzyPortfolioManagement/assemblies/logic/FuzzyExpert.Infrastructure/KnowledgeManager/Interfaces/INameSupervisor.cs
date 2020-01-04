using System.Collections.Generic;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces
{
    public interface INameSupervisor
    {
        void AssignNames(List<UnaryStatement> unaryStatements);
    }
}