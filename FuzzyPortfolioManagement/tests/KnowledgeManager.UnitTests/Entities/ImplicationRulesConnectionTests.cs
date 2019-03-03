using KnowledgeManager.Entities;
using NUnit.Framework;

namespace KnowledgeManager.UnitTests.Entities
{
    [TestFixture]
    public class ImplicationRulesConnectionTests
    {
        private int _connectedRuleNumber = 5;
        private ImplicationRulesConnection _implicationRulesConnection;

        [SetUp]
        public void SetUp()
        {
            _implicationRulesConnection = new ImplicationRulesConnection(_connectedRuleNumber);
        }

        [Test]
        public void ConnectedRuleNumber_GetterReturnsValue()
        {
            // Act
            int actualConnectedRuleNumber = _implicationRulesConnection.ConnectedRuleNumber;

            // Assert
            Assert.AreEqual(_connectedRuleNumber, actualConnectedRuleNumber);
        }

        [Test]
        public void IsReached_ReturnsDefaultValue()
        {
            // Act
            bool actualIsReached = _implicationRulesConnection.IsReached;

            // Assert
            Assert.AreEqual(false, actualIsReached);
        }

        [Test]
        public void IsReached_SetterWorksProperly()
        {
            // Assert
            bool expectedIsReached = true;

            // Act
            _implicationRulesConnection.IsReached = expectedIsReached;

            // Assert
            Assert.AreEqual(expectedIsReached, _implicationRulesConnection.IsReached);
        }

        [Test]
        public void IsReached_GetterReturnsValue()
        {
            // Assert
            bool expectedIsReached = true;
            _implicationRulesConnection.IsReached = expectedIsReached;

            // Act
            bool actualIsReached = _implicationRulesConnection.IsReached;

            // Assert
            Assert.AreEqual(expectedIsReached, actualIsReached);
        }
    }
}