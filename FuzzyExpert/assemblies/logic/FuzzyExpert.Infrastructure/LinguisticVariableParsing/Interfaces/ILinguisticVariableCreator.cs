using System.Collections.Generic;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces
{
    public interface ILinguisticVariableCreator
    {
        List<LinguisticVariable> CreateLinguisticVariableEntities(string linguisticVariable);
    }
}