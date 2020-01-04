using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Core.FuzzificationEngine.Interfaces
{
    public interface IFuzzyEngine
    {
        MembershipFunction Fuzzify(LinguisticVariable variable, double inputValue);
    }
}