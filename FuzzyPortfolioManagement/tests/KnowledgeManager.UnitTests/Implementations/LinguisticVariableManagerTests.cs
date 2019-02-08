using System;
using KnowledgeManager.Implementations;
using NUnit.Framework;

namespace KnowledgeManager.UnitTests.Implementations
{
    [TestFixture]
    public class LinguisticVariableManagerTests
    {
        // Other tests are Integrational

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLinguisticVariableProviderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new LinguisticVariableManager(null); });
        }
    }
}
