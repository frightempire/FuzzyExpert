using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using MembershipFunctionParser.Implementations;

namespace LinguisticVariableParser.Entities
{
    public class LinguisticVariable
    {
        public LinguisticVariable(string variableName, MembershipFunctionList membershipFunctionList, bool isInitialData)
        {
            ExceptionAssert.IsEmpty(variableName);
            ExceptionAssert.IsNull(membershipFunctionList);
            ExceptionAssert.IsEmpty(membershipFunctionList);

            VariableName = variableName;
            MembershipFunctionList = membershipFunctionList;
            IsInitialData = isInitialData;
        }

        public string VariableName { get; }

        public MembershipFunctionList MembershipFunctionList { get; }

        public bool IsInitialData { get; }

        public double MinValue()
        {
            List<double> firstPoints = MembershipFunctionList.Select(mf => mf.PointsList.First()).ToList();
            return firstPoints.Min();
        }

        public double MaxValue()
        {
            List<double> lastPoints = MembershipFunctionList.Select(mf => mf.PointsList.Last()).ToList();
            return lastPoints.Max();
        }

        public double ValueRange()
        {
            return MaxValue() - MinValue();
        }
    }
}