using FuzzyExpert.Application.Entities;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces
{
    public interface IImplicationRuleValidator
    {
        ValidationOperationResult ValidateImplicationRule(string implicationRule);
    }
}