using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;

namespace FuzzificationEngine.Interfaces
{
    public interface IFuzzyEngine
    {
        MembershipFunction Fuzzify(LinguisticVariable variable, double inputValue);
    }
}