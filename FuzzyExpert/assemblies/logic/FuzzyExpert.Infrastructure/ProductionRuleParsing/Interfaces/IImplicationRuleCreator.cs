using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces
{
    public interface IImplicationRuleCreator
    {
        ImplicationRule CreateImplicationRuleEntity(string implicationRule);
    }
}