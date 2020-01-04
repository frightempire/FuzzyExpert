using System;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class ImplicationRuleManagerTests
    {
        // Other tests are Integration

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRuleProviderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new ImplicationRuleManager(null); });
        }
    }
}