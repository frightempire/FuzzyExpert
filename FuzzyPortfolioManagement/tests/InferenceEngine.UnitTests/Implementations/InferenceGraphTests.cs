using System;
using System.Collections.Generic;
using InferenceEngine.Implementations;
using NUnit.Framework;
using ProductionRuleParser.Enums;

namespace InferenceEngine.UnitTests.Implementations
{
    //Other tests are integrational

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
        public void AddRules_ThrowsArgumentNullException_IfNodeListsAreNullOrEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => { _inferenceGraph.AddRule(null, LogicalOperation.And, new List<string> { "then" }); });
            Assert.Throws<ArgumentNullException>(
                () => { _inferenceGraph.AddRule(new List<string>(), LogicalOperation.And, new List<string> { "then" }); });
            Assert.Throws<ArgumentNullException>(
                () => { _inferenceGraph.AddRule(new List<string> { "if" }, LogicalOperation.And, null); });
            Assert.Throws<ArgumentNullException>(
                () => { _inferenceGraph.AddRule(new List<string> {"if"}, LogicalOperation.And, new List<string>()); });
        }
    }
}