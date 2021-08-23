using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.FuzzificationEngine.Interfaces;
using FuzzyExpert.Core.InferenceEngine.Interfaces;

namespace FuzzyExpert.Application.InferenceExpert.Implementations
{
    public class FuzzyExpert : IExpert
    {
        private readonly IDataProvider _initialDataProvider;
        private readonly IKnowledgeBaseManager _knowledgeManager;
        private readonly IInferenceEngine _inferenceEngine;
        private readonly IFuzzyEngine _fuzzyEngine;

        public FuzzyExpert(
            IDataProvider dataProvider,
            IKnowledgeBaseManager knowledgeBaseManager,
            IInferenceEngine inferenceEngine,
            IFuzzyEngine fuzzyEngine)
        {
            _initialDataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _knowledgeManager = knowledgeBaseManager ?? throw new ArgumentNullException(nameof(knowledgeBaseManager));
            _inferenceEngine = inferenceEngine ?? throw new ArgumentNullException(nameof(inferenceEngine));
            _fuzzyEngine = fuzzyEngine ?? throw new ArgumentNullException(nameof(fuzzyEngine));
        }

        public ExpertOpinion GetResult(string profileName)
        {
            var initialData = _initialDataProvider.GetInitialData();
            var knowledgeBase = _knowledgeManager.GetKnowledgeBase(profileName);

            var opinion = new ExpertOpinion();
            if (!initialData.IsPresent)
            {
                opinion.AddErrorMessage("Initial data is not consistent. Check logs for more information.");
            }
            if (!knowledgeBase.IsPresent)
            {
                opinion.AddErrorMessage("Knowledge base is not consistent. Check logs for more information.");
            }

            ValidateInitialDataAgainstKnowledgeBase(initialData, knowledgeBase, opinion);
            if (!opinion.IsSuccess)
            {
                return opinion;
            }

            FillInferenceEngineRules(knowledgeBase.Value);
            var activatedNodes = GetInitialNodes(knowledgeBase.Value, initialData.Value);
            var inferenceResults = _inferenceEngine.GetInferenceResults(activatedNodes);
            opinion.AddResults(DeFuzzifyResults(inferenceResults, knowledgeBase.Value.LinguisticVariables));
            return opinion;
        }

        private List<DeFuzzifiedInferenceResult> DeFuzzifyResults(
            List<InferenceResult> inferenceResults,
            Dictionary<int, LinguisticVariable> linguisticVariables)
        {
            var deFuzzifiedInferenceResult = new List<DeFuzzifiedInferenceResult>();
            foreach (var inferenceResult in inferenceResults)
            {
                var variableName = inferenceResult.NodeName.Split('=')[0].Trim();
                var fuzzyValue = inferenceResult.NodeName.Split('=')[1].Trim();
                var matchingVariable = linguisticVariables.First(lv => lv.Value.VariableName == variableName).Value;
                var matchingMembershipFunction = matchingVariable.MembershipFunctionList.FindByVariableName(fuzzyValue);
                var deFuzzifiedValue = matchingMembershipFunction.CenterOfGravity();
                deFuzzifiedInferenceResult.Add(
                    new DeFuzzifiedInferenceResult(
                        inferenceResult.NodeName, inferenceResult.ConfidenceFactor, deFuzzifiedValue));
            }
            return deFuzzifiedInferenceResult;
        }

        private void ValidateInitialDataAgainstKnowledgeBase(
            Optional<List<InitialData>> initialData,
            Optional<KnowledgeBase> knowledgeBase,
            ExpertOpinion opinion)
        {
            foreach (var data in initialData.Value)
            {
                var matchingVariable = knowledgeBase.Value.LinguisticVariables.SingleOrDefault(lv => lv.Value.VariableName == data.Name);
                if (matchingVariable.Value == null)
                {
                    opinion.AddErrorMessage($"Initial data {data.Name} is not present in linguistic variables base.");
                }
            }
        }

        private List<InitialData> GetInitialNodes(KnowledgeBase knowledgeBase, List<InitialData> initialData)
        {
            var ifUnaryStatements = knowledgeBase.ImplicationRules
                .SelectMany(ir => ir.Value.IfStatement.SelectMany(ifs => ifs.UnaryStatements))
                .ToList();

            var activatedNodes = new List<InitialData>();

            foreach (var data in initialData)
            {
                var matchingVariable = knowledgeBase.LinguisticVariables.SingleOrDefault(lv => lv.Value.VariableName == data.Name);
                var relatedStatementNames = knowledgeBase.LinguisticVariablesRelations
                    .SingleOrDefault(lvr => lvr.LinguisticVariableNumber == matchingVariable.Key)
                    ?.RelatedUnaryStatementNames;

                var membershipFunction = _fuzzyEngine.Fuzzify(matchingVariable.Value, data.Value);

                foreach (var relatedStatementName in relatedStatementNames)
                {
                    var matchingStatement = ifUnaryStatements.FirstOrDefault(ius => 
                        ius.ToString() == relatedStatementName && 
                        ius.RightOperand == membershipFunction.LinguisticVariableName);
                    if (matchingStatement != null)
                    {
                        activatedNodes.Add(new InitialData(relatedStatementName, data.Value, data.ConfidenceFactor));
                    }
                }
            }
            return activatedNodes;
        }

        private void FillInferenceEngineRules(KnowledgeBase knowledgeBase)
        {
            foreach (var implicationRule in knowledgeBase.ImplicationRules)
            {
                var ifNodeNames = implicationRule.Value.IfStatement
                    .SelectMany(ifs => ifs.UnaryStatements.Select(us => us.ToString()))
                    .ToList();
                var operation = ifNodeNames.Count == 1 ? LogicalOperation.None : LogicalOperation.And;
                var thenNodeNames = implicationRule.Value.ThenStatement.UnaryStatements
                    .Select(us => us.ToString())
                    .ToList();
                _inferenceEngine.AddRule(ifNodeNames, operation, thenNodeNames);
            }
        }
    }
}