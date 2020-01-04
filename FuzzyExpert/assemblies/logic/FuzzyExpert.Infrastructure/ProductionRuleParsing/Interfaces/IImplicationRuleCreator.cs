using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Entities;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces
{
    public interface IImplicationRuleCreator
    {
        ImplicationRule CreateImplicationRuleEntity(ImplicationRuleStrings implicationRuleStrings);
    }
}