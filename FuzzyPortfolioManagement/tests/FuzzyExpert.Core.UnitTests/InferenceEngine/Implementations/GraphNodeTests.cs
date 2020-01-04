using System;
using System.Collections.Generic;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using FuzzyExpert.Core.InferenceEngine.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Core.UnitTests.InferenceEngine.Implementations
{
    [TestFixture]
    public class GraphNodeTests
    {
        private const string NodeName = "testNode";
        private GraphNode _graphNode;

        [SetUp]
        public void SetUp()
        {
            _graphNode = new GraphNode(NodeName, new Dictionary<string, double> ());
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new GraphNode(null, new Dictionary<string, double> ()); });
            Assert.Throws<ArgumentNullException>(() => { new GraphNode(string.Empty, new Dictionary<string, double>()); });
            Assert.Throws<ArgumentNullException>(() => { new GraphNode(NodeName, null); });
        }

        [Test]
        public void Properties_DefaultValues()
        {
            Assert.AreEqual(0, _graphNode.ConfidenceFactor);
            Assert.IsNotNull(_graphNode.RelatedRules);
            Assert.AreEqual(_graphNode.RelatedRules.Count, 0);
        }

        [Test]
        public void ConfidenceFactor_GetterReturnsValue()
        {
            // Arrange
            double expectedConfidenceFactor = 10;
            _graphNode.UpdateConfidenceFactor(expectedConfidenceFactor);

            // Assert
            Assert.AreEqual(expectedConfidenceFactor, _graphNode.ConfidenceFactor);
        }

        [Test]
        public void Name_GetterReturnsValue()
        {
            // Act
            string actualNodeName = _graphNode.Name;

            // Assert
            Assert.AreEqual(NodeName, actualNodeName);
        }

        [Test]
        public void RelatedRules_GetterReturnsValue()
        {
            // Arrange
            var inferenceRuleMock = MockRepository.GenerateMock<IInferenceRule>();
            _graphNode.RelatedRules.Add(inferenceRuleMock);

            // Assert
            Assert.AreEqual(_graphNode.RelatedRules.Count, 1);
        }

        [Test]
        public void UpdateConfidenceFactor_UpdatesConfidenceFactor()
        {
            // Arrange
            double expectedConfidenceFactor = 10;
            var inferenceRuleMock = MockRepository.GenerateMock<IInferenceRule>();
            _graphNode.RelatedRules.Add(inferenceRuleMock);

            // Act
            _graphNode.UpdateConfidenceFactor(expectedConfidenceFactor);

            // Assert
            Assert.AreEqual(expectedConfidenceFactor, _graphNode.ConfidenceFactor);
            inferenceRuleMock.AssertWasCalled(x => x.UpdateConfidenceFactor());
        }
    }
}