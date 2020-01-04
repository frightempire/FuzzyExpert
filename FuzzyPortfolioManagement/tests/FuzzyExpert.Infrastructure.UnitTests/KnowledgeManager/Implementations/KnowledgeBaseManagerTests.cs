using System;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    // Other tests are Integration
    [TestFixture]
    public class KnowledgeBaseManagerTests
    {
        private IImplicationRuleManager _implicationRuleManagerMock;
        private ILinguisticVariableManager _linguisticVariableManagerMock;
        private IKnowledgeBaseValidator _knowledgeBaseValidatorMock;
        private ILinguisticVariableRelationsInitializer _linguisticVariableRelationsInitializer;
        private IValidationOperationResultLogger _validationOperationResultLoggerMock;
        private KnowledgeBaseManager _knowledgeBaseManager;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleManagerMock = MockRepository.GenerateMock<IImplicationRuleManager>();
            _linguisticVariableManagerMock = MockRepository.GenerateMock<ILinguisticVariableManager>();
            _knowledgeBaseValidatorMock = MockRepository.GenerateMock<IKnowledgeBaseValidator>();
            _linguisticVariableRelationsInitializer = MockRepository.GenerateMock<ILinguisticVariableRelationsInitializer>();
            _validationOperationResultLoggerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();

            _knowledgeBaseManager = new KnowledgeBaseManager(
                _implicationRuleManagerMock,
                _linguisticVariableManagerMock,
                _knowledgeBaseValidatorMock,
                _linguisticVariableRelationsInitializer,
                _validationOperationResultLoggerMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    null,
                    _linguisticVariableManagerMock,
                    _knowledgeBaseValidatorMock,
                    _linguisticVariableRelationsInitializer,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    null,
                    _knowledgeBaseValidatorMock,
                    _linguisticVariableRelationsInitializer,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    _linguisticVariableManagerMock,
                    null,
                    _linguisticVariableRelationsInitializer,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    _linguisticVariableManagerMock,
                    _knowledgeBaseValidatorMock,
                    null,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new KnowledgeBaseManager(
                    _implicationRuleManagerMock,
                    _linguisticVariableManagerMock,
                    _knowledgeBaseValidatorMock,
                    _linguisticVariableRelationsInitializer,
                    null);
            });
        }
    }
}