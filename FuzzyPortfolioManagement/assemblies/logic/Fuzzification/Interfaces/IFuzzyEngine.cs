using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;

namespace Fuzzification.Interfaces
{
    public interface IFuzzyEngine
    {
        MembershipFunction Fuzzify(LinguisticVariable variable, double inputValue);
    }
}