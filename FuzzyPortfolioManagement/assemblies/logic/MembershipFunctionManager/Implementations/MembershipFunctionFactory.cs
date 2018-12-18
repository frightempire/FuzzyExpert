using System;
using System.Collections.Generic;
using System.ComponentModel;
using MembershipFunctionManager.Entities;
using MembershipFunctionManager.Enums;
using MembershipFunctionManager.Interfaces;

namespace MembershipFunctionManager.Implementations
{
    public class MembershipFunctionFactory : IMembershipFunctionFactory
    {
        public MembershipFunction CreateMembershipFunction(MembershipFunctionType membershipFunctionType, string linguisticVariableName, List<double> points)
        {
            int countOfPoints = points.Count;
            switch (membershipFunctionType)
            {
                case MembershipFunctionType.Trapesoidal:
                    if (countOfPoints != 4)
                        throw new ArgumentOutOfRangeException($"Trapesoidal membership function contains {countOfPoints} points instead of 4.");
                    return new TrapezoidalMembershipFunction(linguisticVariableName, points[0], points[1], points[2], points[3]);

                default:
                    throw new InvalidEnumArgumentException($"Not supported membership function type: {membershipFunctionType}.");
            }
        }
    }
}