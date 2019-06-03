using System;
using System.Collections.Generic;
using InferenceEngine.Implementations;
using InferenceEngine.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace InferenceEngine.UnitTests.Implementations
{
    [TestFixture]
    public class GraphNodeTests
    {
        private const string NodeName = "testNode";
        private GraphNode _graphNode;

        [SetUp]
        public void SetUp()
        {
            _graphNode = new GraphNode(NodeName, new List<string>());
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new GraphNode(null, new List<string>()); });
            Assert.Throws<ArgumentNullException>(() => { new GraphNode(string.Empty, new List<string>()); });
            Assert.Throws<ArgumentNullException>(() => { new GraphNode(NodeName, null); });
        }

        [Test]
        public void Properties_DefaultValues()
        {
            Assert.IsFalse(_graphNode.Active);
            Assert.IsNotNull(_graphNode.RelatedRules);
            Assert.AreEqual(_graphNode.RelatedRules.Count, 0);
        }

        [Test]
        public void Status_GetterReturnsValue()
        {
            // Arrange
            _graphNode.ActivateNode();

            // Assert
            Assert.IsTrue(_graphNode.Active);
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
        public void UpdateStatus_UpdatesStatuses()
        {
            // Arrange
            var inferenceRuleMock = MockRepository.GenerateMock<IInferenceRule>();
            _graphNode.RelatedRules.Add(inferenceRuleMock);

            // Act
            _graphNode.ActivateNode();

            // Assert
            Assert.IsTrue(_graphNode.Active);
            inferenceRuleMock.AssertWasCalled(x => x.UpdateStatus());
        }
    }
}