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
            Optional<List<InitialData>> initialData = _initialDataProvider.GetInitialData();
            Optional<KnowledgeBase> knowledgeBase = _knowledgeManager.GetKnowledgeBase(profileName);

            ExpertOpinion opinion = new ExpertOpinion();
            if (!initialData.IsPresent) opinion.AddErrorMessage("Initial data is not consistent. Check logs for more information.");
            if (!knowledgeBase.IsPresent) opinion.AddErrorMessage("Knowledge base is not consistent. Check logs for more information.");
            ValidateInitialDataAgainstKnowledgeBase(initialData, knowledgeBase, opinion);

            if (!opinion.IsSuccess) return opinion;

            FillInferenceEngineRules(knowledgeBase.Value);
            List<InitialData> activatedNodes = GetInitialNodes(knowledgeBase.Value, initialData.Value);
            opinion.AddResults(_inferenceEngine.GetInferenceResults(activatedNodes));
            return opinion;
        }

        private void ValidateInitialDataAgainstKnowledgeBase(
            Optional<List<InitialData>> initialData,
            Optional<KnowledgeBase> knowledgeBase,
            ExpertOpinion opinion)
        {
            foreach (var data in initialData.Value)
            {
                KeyValuePair<int, LinguisticVariable> matchingVariable =
                    knowledgeBase.Value.LinguisticVariables.SingleOrDefault(lv => lv.Value.VariableName == data.Name);
                if (matchingVariable.Value == null)
                {
                    opinion.AddErrorMessage($"Initial data {data.Name} is not present in linguistic variables base.");
                }
            }
        }

        private List<InitialData> GetInitialNodes(KnowledgeBase knowledgeBase, List<InitialData> initialData)
        {
            List<UnaryStatement> ifUnaryStatements = knowledgeBase.ImplicationRules
                .SelectMany(ir => ir.Value.IfStatement.SelectMany(ifs => ifs.UnaryStatements))
                .ToList();

            List<InitialData> activatedNodes = new List<InitialData>();

            foreach (var data in initialData)
            {
                KeyValuePair<int, LinguisticVariable> matchingVariable = knowledgeBase.LinguisticVariables
                    .SingleOrDefault(lv => lv.Value.VariableName == data.Name);
                List<string> relatedStatementNames = knowledgeBase.LinguisticVariablesRelations
                    .SingleOrDefault(lvr => lvr.LinguisticVariableNumber == matchingVariable.Key)
                    ?.RelatedUnaryStatementNames;

                MembershipFunction membershipFunction = _fuzzyEngine.Fuzzify(matchingVariable.Value, data.Value);

                foreach (var name in relatedStatementNames)
                {
                    UnaryStatement matchingStatement = ifUnaryStatements
                        .FirstOrDefault(ius => ius.Name == name && ius.RightOperand == membershipFunction.LinguisticVariableName);
                    if (matchingStatement != null) activatedNodes.Add(new InitialData(name, data.Value, data.ConfidenceFactor));
                }
            }
            return activatedNodes;
        }

        private void FillInferenceEngineRules(KnowledgeBase knowledgeBase)
        {
            foreach (var implicationRule in knowledgeBase.ImplicationRules)
            {
                List<string> ifNodeNames = implicationRule.Value.IfStatement
                    .SelectMany(ifs => ifs.UnaryStatements.Select(us => us.Name))
                    .ToList();
                LogicalOperation operation = ifNodeNames.Count == 1 ? LogicalOperation.None : LogicalOperation.And;
                List<string> thenNodeNames = implicationRule.Value.ThenStatement.UnaryStatements
                    .Select(us => us.Name)
                    .ToList();
                _inferenceEngine.AddRule(ifNodeNames, operation, thenNodeNames);
            }
        }
    }
}