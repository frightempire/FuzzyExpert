using System;

namespace FuzzyExpert.Core.Entities
{
    public class TrapezoidalMembershipFunction: MembershipFunction
    {
        /// <param name="linguisticVariableName">The name that identifies linguistic variable.</param>
        /// <param name="x0">The value of the (x0, 0) point.</param>
        /// <param name="x1">The value of the (x1, 1) point.</param>
        /// <param name="x2">The value of the (x2, 1) point.</param>
        /// <param name="x3">The value of the (x3, 0) point.</param>
        public TrapezoidalMembershipFunction(string linguisticVariableName, double x0, double x1, double x2, double x3): base(linguisticVariableName)
        {
            if (x0 > x1 || x1 > x2 || x2 > x3) throw new ArgumentException("Points order is violated.");

            PointsList.Add(x0);
            PointsList.Add(x1);
            PointsList.Add(x2);
            PointsList.Add(x3);
        }

        public override double MembershipDegree(double value)
        {
            if (PointsList[0] <= value && value < PointsList[1]) return (0.1 + value - PointsList[0]) / (PointsList[1] - PointsList[0]);
            if (PointsList[1] <= value && value <= PointsList[2]) return 1;
            if (PointsList[2] < value && value <= PointsList[3]) return (PointsList[3] - value + 0.1) / (PointsList[3] - PointsList[2]);
            return 0;
        }

        #region Equals/GetHashCode

        private const double Precision = 0.00001;

        public override bool Equals(Object objectToCompareWith)
        {
            TrapezoidalMembershipFunction trapezoidalMembershipFunction =
                objectToCompareWith as TrapezoidalMembershipFunction;

            if (trapezoidalMembershipFunction == null) return false;

            return LinguisticVariableName == trapezoidalMembershipFunction.LinguisticVariableName &&
                   Math.Abs(PointsList[0] - trapezoidalMembershipFunction.PointsList[0]) < Precision &&
                   Math.Abs(PointsList[1] - trapezoidalMembershipFunction.PointsList[1]) < Precision &&
                   Math.Abs(PointsList[2] - trapezoidalMembershipFunction.PointsList[2]) < Precision &&
                   Math.Abs(PointsList[3] - trapezoidalMembershipFunction.PointsList[3]) < Precision;
        }

        public override int GetHashCode()
        {
            int hashCode = 13;
            hashCode = (hashCode * 7) + LinguisticVariableName.GetHashCode();
            hashCode = (hashCode * 7) + PointsList[0].GetHashCode();
            hashCode = (hashCode * 7) + PointsList[1].GetHashCode();
            hashCode = (hashCode * 7) + PointsList[2].GetHashCode();
            hashCode = (hashCode * 7) + PointsList[3].GetHashCode();
            return hashCode;
        }

        #endregion
    }
}