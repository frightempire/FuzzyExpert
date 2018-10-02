using System;
using NUnit.Framework;
using ProductionRuleManager.Implementations;

namespace ProductionRuleManager.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleManagerTests
    {
        // Other tests are Integrational

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRuleProviderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new FileImplicationRuleManager(null); });
        }
    }
}
