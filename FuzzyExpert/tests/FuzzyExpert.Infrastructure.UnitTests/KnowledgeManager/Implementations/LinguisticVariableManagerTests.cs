using System;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class LinguisticVariableManagerTests
    {
        // Other tests are Integration

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLinguisticVariableProviderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new LinguisticVariableManager(null); });
        }
    }
}