using System;
using System.Collections.Generic;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Core.UnitTests.InferenceEngine.Implementations
{
    //Other tests are integration

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