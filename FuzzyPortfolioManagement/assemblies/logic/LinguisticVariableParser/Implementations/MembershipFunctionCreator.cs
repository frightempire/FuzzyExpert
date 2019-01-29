using System;
using System.Collections.Generic;
using System.ComponentModel;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Enums;
using LinguisticVariableParser.Interfaces;

namespace LinguisticVariableParser.Implementations
{
    public class MembershipFunctionCreator : IMembershipFunctionCreator
    {
        public MembershipFunction CreateMembershipFunctionEntity(MembershipFunctionType membershipFunctionType, string membershipFunctionName, List<double> points)
        {
            int countOfPoints = points.Count;
            switch (membershipFunctionType)
            {
                case MembershipFunctionType.Trapezoidal:
                    if (countOfPoints != 4)
                        throw new ArgumentOutOfRangeException($"Trapesoidal membership function contains {countOfPoints} points instead of 4.");
                    return new TrapezoidalMembershipFunction(membershipFunctionName, points[0], points[1], points[2], points[3]);

                default:
                    throw new InvalidEnumArgumentException($"Not supported membership function type: {membershipFunctionType}.");
            }
        }
    }
}