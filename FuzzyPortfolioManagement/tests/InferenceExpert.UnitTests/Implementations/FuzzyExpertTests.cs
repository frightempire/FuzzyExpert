using System;
using DataProvider.Interfaces;
using FuzzificationEngine.Interfaces;
using InferenceEngine.Interfaces;
using InferenceExpert.Implementations;
using KnowledgeManager.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace InferenceExpert.UnitTests.Implementations
{
    // Other tests are integrational
    [TestFixture]
    public class FuzzyExpertTests
    {
        private IDataProvider _initialDataProviderMock;
        private IKnowledgeBaseManager _knowledgeManagerMock;
        private IInferenceEngine _inferenceEngineMock;
        private IFuzzyEngine _fuzzyEngineMock;

        [SetUp]
        public void SetUp()
        {
            _initialDataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            _knowledgeManagerMock = MockRepository.GenerateMock<IKnowledgeBaseManager>();
            _inferenceEngineMock = MockRepository.GenerateMock<IInferenceEngine>();
            _fuzzyEngineMock = MockRepository.GenerateMock<IFuzzyEngine>();
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_IfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert(null, _knowledgeManagerMock, _inferenceEngineMock, _fuzzyEngineMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert(_initialDataProviderMock, null, _inferenceEngineMock, _fuzzyEngineMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert(_initialDataProviderMock, _knowledgeManagerMock, null, _fuzzyEngineMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert(_initialDataProviderMock, _knowledgeManagerMock, _inferenceEngineMock, null);
            });
        }
    }
}