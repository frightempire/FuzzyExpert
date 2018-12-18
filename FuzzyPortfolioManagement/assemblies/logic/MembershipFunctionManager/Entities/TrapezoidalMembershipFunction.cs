using System;

namespace MembershipFunctionManager.Entities
{
    public class TrapezoidalMembershipFunction: MembershipFunction
    {
        /// <param name="linguisticVariableName">The name that identificates the linguistic variable.</param>
        /// <param name="x0">The value of the (x0, 0) point.</param>
        /// <param name="x1">The value of the (x1, 1) point.</param>
        /// <param name="x2">The value of the (x2, 1) point.</param>
        /// <param name="x3">The value of the (x3, 0) point.</param>
        public TrapezoidalMembershipFunction(string linguisticVariableName, double x0, double x1, double x2, double x3): base(linguisticVariableName)
        {
            if (x0 > x1 || x1 > x2 || x2 > x3)
                throw new ArgumentException("Points order is violdated.");

            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
        }

        public double X0 { get; }

        public double X1 { get; }

        public double X2 { get; }

        public double X3 { get; }



        #region Equals/GetHashCode

        private const double Precision = 0.00001;

        public override bool Equals(Object objectToCompareWith)
        {
            TrapezoidalMembershipFunction trapezoidalMembershipFunction =
                objectToCompareWith as TrapezoidalMembershipFunction;

            if (trapezoidalMembershipFunction == null) return false;

            return LinguisticVariableName == trapezoidalMembershipFunction.LinguisticVariableName &&
                   Math.Abs(X0 - trapezoidalMembershipFunction.X0) < Precision &&
                   Math.Abs(X1 - trapezoidalMembershipFunction.X1) < Precision &&
                   Math.Abs(X2 - trapezoidalMembershipFunction.X2) < Precision &&
                   Math.Abs(X3 - trapezoidalMembershipFunction.X3) < Precision;
        }

        public override int GetHashCode()
        {
            int hashCode = 13;
            hashCode = (hashCode * 7) + LinguisticVariableName.GetHashCode();
            hashCode = (hashCode * 7) + X0.GetHashCode();
            hashCode = (hashCode * 7) + X1.GetHashCode();
            hashCode = (hashCode * 7) + X2.GetHashCode();
            hashCode = (hashCode * 7) + X3.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}