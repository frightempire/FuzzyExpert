using System;
using CommonLogic.Interfaces;
using KnowledgeManager.Implementations;
using KnowledgeManager.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace KnowledgeManager.UnitTests.Implementations
{
    [TestFixture]
    public class KnowledgeBaseManagerTests
    {
        // Other tests are Integrational

        private IImplicationRuleManager _implicationRuleManagerMock;
        private ILinguisticVariableManager _linguisticVariableManagerMock;
        private IKnowledgeBaseValidator _knowledgeBaseValidatorMock;
        private IValidationOperationResultLogger _validationOperationResultLoggerMock;

        private KnowledgeBaseManager _knowledgeBaseManager;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleManagerMock = MockRepository.GenerateMock<IImplicationRuleManager>();
            _linguisticVariableManagerMock = MockRepository.GenerateMock<ILinguisticVariableManager>();
            _knowledgeBaseValidatorMock = MockRepository.GenerateMock<IKnowledgeBaseValidator>();
            _validationOperationResultLoggerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();

            _knowledgeBaseManager = new KnowledgeBaseManager(
                _implicationRuleManagerMock,
                _linguisticVariableManagerMock,
                _knowledgeBaseValidatorMock,
                _validationOperationResultLoggerMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRuleManagerIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    null,
                    _linguisticVariableManagerMock,
                    _knowledgeBaseValidatorMock,
                    _validationOperationResultLoggerMock);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLinguisticVariableManagerIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    null,
                    _knowledgeBaseValidatorMock,
                    _validationOperationResultLoggerMock);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfKnowledgeBaseValidatorIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    _linguisticVariableManagerMock,
                    null,
                    _validationOperationResultLoggerMock);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfValidationOperationResultLoggerIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    _linguisticVariableManagerMock,
                    _knowledgeBaseValidatorMock,
                    null);
            });
        }
    }
}