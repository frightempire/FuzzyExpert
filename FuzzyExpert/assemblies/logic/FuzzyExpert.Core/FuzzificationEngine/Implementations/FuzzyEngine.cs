using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.FuzzificationEngine.Interfaces;

namespace FuzzyExpert.Core.FuzzificationEngine.Implementations
{
    public class FuzzyEngine : IFuzzyEngine
    {
        public MembershipFunction Fuzzify(LinguisticVariable variable, double inputValue)
        {
            MembershipFunction function = null;
            double finalDegree = -1;

            foreach (var membershipFunction in variable.MembershipFunctionList)
            {
                var membershipDegree = membershipFunction.MembershipDegree(inputValue);
                if (membershipDegree < finalDegree)
                {
                    continue;
                }

                finalDegree = membershipDegree;
                function = membershipFunction;
            }

            return function;
        }
    }
}