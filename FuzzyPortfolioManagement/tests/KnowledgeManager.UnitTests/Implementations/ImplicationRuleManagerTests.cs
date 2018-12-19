using System;
using KnowledgeManager.Implementations;
using NUnit.Framework;

namespace KnowledgeManager.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleManagerTests
    {
        // Other tests are Integrational

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRuleProviderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new ImplicationRuleManager(null); });
        }
    }
}
