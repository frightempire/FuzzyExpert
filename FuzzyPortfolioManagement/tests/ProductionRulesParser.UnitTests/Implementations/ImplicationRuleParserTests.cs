using System.Collections.Generic;
using NUnit.Framework;
using ProductionRulesParser.Implementations;
using ProductionRulesParser.Interfaces;
using Rhino.Mocks;

namespace ProductionRulesParser.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleParserTests
    {
        private IImplicationRuleHelper _implicationRuleHelper;
        private ImplicationRuleParser _implicationRuleParser;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleHelper = MockRepository.GenerateMock<IImplicationRuleHelper>();
            _implicationRuleParser = new ImplicationRuleParser(_implicationRuleHelper);
        }

        [Test, Ignore("To be implemented")]
        public void DivideProductionRuleString()
        {
            // Arrange
            string productionRule = "A=a | (B=b & C=c)";

            // Act
            List<string> result = _implicationRuleParser.DivideImplicationRule(productionRule);
        }
    }
}
