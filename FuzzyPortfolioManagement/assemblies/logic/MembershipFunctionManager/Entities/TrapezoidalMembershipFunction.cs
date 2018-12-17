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
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
        }

        public double X0 { get; }

        public double X1 { get; }

        public double X2 { get; }

        public double X3 { get; }
    }
}