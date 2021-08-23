using System;
using System.Collections.Generic;

namespace FuzzyExpert.Core.Entities
{
    public abstract class MembershipFunction
    {
        protected MembershipFunction(string linguisticVariableName)
        {
            if (string.IsNullOrWhiteSpace(linguisticVariableName)) throw new ArgumentNullException(nameof(linguisticVariableName));

            LinguisticVariableName = linguisticVariableName;
            PointsList = new List<double>();
        }

        public string LinguisticVariableName { get; }

        public List<double> PointsList { get; }

        public abstract double MembershipDegree(double value);

        public abstract double CenterOfGravity();
    }
}