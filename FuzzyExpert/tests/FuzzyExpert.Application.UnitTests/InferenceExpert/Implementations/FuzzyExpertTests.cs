using System;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Core.FuzzificationEngine.Interfaces;
using FuzzyExpert.Core.InferenceEngine.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Application.UnitTests.InferenceExpert.Implementations
{
    // Other tests are integration
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
                new FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert(null, _knowledgeManagerMock, _inferenceEngineMock, _fuzzyEngineMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert(_initialDataProviderMock, null, _inferenceEngineMock, _fuzzyEngineMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert(_initialDataProviderMock, _knowledgeManagerMock, null, _fuzzyEngineMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert(_initialDataProviderMock, _knowledgeManagerMock, _inferenceEngineMock, null);
            });
        }
    }
}