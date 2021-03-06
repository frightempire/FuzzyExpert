﻿using System;
using System.Collections.Generic;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;

namespace FuzzyExpert.Core.InferenceEngine.Interfaces
{
    public interface IInferenceEngine
    {
        void AddRule(List<string> ifNodeNames, LogicalOperation operation, List<string> thenNodeNames);

        List<Tuple<string, double>> GetInferenceResults(List<InitialData> initialData);
    }
}