using System.Collections.Generic;
using CommonLogic;

namespace MembershipFunctionParser.Entities
{
    public abstract class MembershipFunction
    {
        protected MembershipFunction(string linguisticVariableName)
        {
            ExceptionAssert.IsEmpty(linguisticVariableName);

            LinguisticVariableName = linguisticVariableName;
            PointsList = new List<double>();
        }

        public string LinguisticVariableName { get; }

        public List<double> PointsList { get; }
    }
}