using System.Collections.Generic;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.IntegrationTests
{
    [TestFixture]
    public class InferenceGraphTests
    {
        private InferenceGraph _inferenceGraph;

        [SetUp]
        public void SetUp()
        {
            _inferenceGraph = new InferenceGraph();
        }

        [Test]
        public void GetInferenceResults_ReturnsCorrectInferenceResult_Case1()
        {
            // Arrange
            List<InitialData> initialData = new List<InitialData>
            {
                new InitialData("init1_1", 100, 0.1),
                new InitialData("init1_2", 100, 0.9),
                new InitialData("init2_1", 100, 0.4),
                new InitialData("init3_1", 100, 0.3),
                new InitialData("init4_1", 100, 0.9)
            };

            _inferenceGraph.AddRule(new List<string> { "init1_1", "init1_2" }, LogicalOperation.And, new List<string> { "A1" });
            _inferenceGraph.AddRule(new List<string> { "init2_1" }, LogicalOperation.None, new List<string> { "A2" });
            _inferenceGraph.AddRule(new List<string> { "init1_1" }, LogicalOperation.None, new List<string> { "A3" });
            _inferenceGraph.AddRule(new List<string> { "init1_2", "init3_1" }, LogicalOperation.Or, new List<string> { "A4" });
            _inferenceGraph.AddRule(new List<string> { "init4_1" }, LogicalOperation.None, new List<string> { "A4" });
            _inferenceGraph.AddRule(new List<string> { "init2_1", "init4_1" }, LogicalOperation.And, new List<string> { "B1" });
            _inferenceGraph.AddRule(new List<string> { "A1", "A2" }, LogicalOperation.And, new List<string> { "B2", "B5" });
            _inferenceGraph.AddRule(new List<string> { "init1_1", "A2" }, LogicalOperation.And, new List<string> { "B3" });
            _inferenceGraph.AddRule(new List<string> { "A3", "A4" }, LogicalOperation.Or, new List<string> { "B4" });
            _inferenceGraph.AddRule(new List<string> { "A4", "B1" }, LogicalOperation.And, new List<string> { "F2" });
            _inferenceGraph.AddRule(new List<string> { "B2", "B3" }, LogicalOperation.Or, new List<string> { "F1", "F6" });
            _inferenceGraph.AddRule(new List<string> { "init2_1", "B4" }, LogicalOperation.And, new List<string> { "F1" });
            _inferenceGraph.AddRule(new List<string> { "B5", "B3" }, LogicalOperation.And, new List<string> { "F3" });

            Dictionary<string, double> expectedInferenceResult = new Dictionary<string, double>
            {
                {"init1_1" , 0.1 },
                {"A3" , 0.1 },
                {"init1_2" , 0.9 },
                {"A1" , 0.1 },
                {"init2_1" , 0.4 },
                {"A2" , 0.4 },
                {"B2" , 0.1 },
                {"B5" , 0.1 },
                {"B3" , 0.1 },
                {"F1" , 0.1 },
                {"F6" , 0.1 },
                {"F3" , 0.1 },
                {"init3_1" , 0.3 },
                {"A4" , 0.9 },
                {"B4" , 0.9 },
                {"init4_1" , 0.9 },
                {"B1" , 0.4 },
                {"F2" , 0.4 }
            };

            // Act
            Dictionary<string, double> actualInferenceResult = _inferenceGraph.GetInferenceResults(initialData);

            // Assert
            Assert.AreEqual(expectedInferenceResult, actualInferenceResult);
        }
    }
}