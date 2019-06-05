using System.Collections.Generic;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface INameSupervisor
    {
        void AssignNames(List<UnaryStatement> unaryStatements);
    }
}