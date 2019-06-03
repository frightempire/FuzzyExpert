using System.Collections.Generic;
using InferenceEngine.Implementations;
using NUnit.Framework;
using ProductionRuleParser.Enums;

namespace IntegrationTests
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
        public void GetInferenceResults_ReturnsCorrectListOfStatusChanges()
        {
            // Arrange
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

            List<string> expectedsStatusChanges = new List<string>
            {
                "init1_1", "A3", "B4", "init2_1", "A2", "B3", "F1", "F6"
            };

            // Act
            List<string> actualStatusChanges = _inferenceGraph.GetInferenceResults(new List<string> {"init1_1", "init2_1"});

            // Assert
            Assert.AreEqual(expectedsStatusChanges, actualStatusChanges);
        }
    }
}