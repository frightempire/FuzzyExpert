using FuzzificationEngine.Interfaces;
using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;

namespace FuzzificationEngine.Implementaions
{
    public class FuzzyEngine : IFuzzyEngine
    {
        public MembershipFunction Fuzzify(LinguisticVariable variable, double inputValue)
        {
            MembershipFunction function = null;
            double finalDegree = -1;

            foreach (MembershipFunction membershipFunction in variable.MembershipFunctionList)
            {
                double membershipDegree = membershipFunction.MembershipDegree(inputValue);
                if (membershipDegree < finalDegree) continue;

                finalDegree = membershipDegree;
                function = membershipFunction;
            }

            return function;
        }
    }
}