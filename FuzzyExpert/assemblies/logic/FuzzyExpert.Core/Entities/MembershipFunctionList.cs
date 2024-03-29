﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyExpert.Core.Entities
{
    public class MembershipFunctionList: List<MembershipFunction>
    {
        public MembershipFunction FindByVariableName(string variableName)
        {
            ValidateVariableNameInList(variableName);
            return this.First(mf => mf.LinguisticVariableName.Trim() == variableName);
        }

        private List<string> VariableNames => this.Select(mf => mf.LinguisticVariableName.Trim()).ToList();

        private void ValidateVariableNameInList(string variableName)
        {
            if (VariableNames.Count == 0)
                throw new ArgumentNullException(nameof(VariableNames));

            if (!VariableNames.Contains(variableName))
                throw new ArgumentException($"There's no membership function with variable {variableName}.");

            if (VariableNames.Count(v => v == variableName) > 1)
                throw new ArgumentOutOfRangeException($"Membership function with variable {variableName} repeated multiple times.");
        }
    }
}