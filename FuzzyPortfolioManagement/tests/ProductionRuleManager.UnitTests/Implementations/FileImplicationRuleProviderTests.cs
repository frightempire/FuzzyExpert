﻿using System;
using System.Collections.Generic;
using System.IO;
using CommonLogic.Interfaces;
using NUnit.Framework;
using ProductionRuleManager.Implementations;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;
using ProductionRulesParser.Interfaces;
using Rhino.Mocks;

namespace ProductionRuleManager.UnitTests.Implementations
{
    [TestFixture]
    public class FileImplicationRuleProviderTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\TestFile.txt");

        private IFileReader _fileReaderMock;
        private IImplicationRuleCreator _implicationRuleCreatorMock;

        private FileImplicationRuleProvider _fileImplicationRuleProvider;

        [SetUp]
        public void SetUp()
        {
            _fileReaderMock = MockRepository.GenerateMock<IFileReader>();
            _implicationRuleCreatorMock = MockRepository.GenerateMock<IImplicationRuleCreator>();

            _fileImplicationRuleProvider = new FileImplicationRuleProvider(_fileReaderMock, _implicationRuleCreatorMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfFileReaderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(null, _implicationRuleCreatorMock);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRuleCreatorIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(_fileReaderMock, null);
            });
        }

        [Test]
        public void FilePath_SetterWorksProperly()
        {
            // Arrange
            string expectedFilePath = "SomeFile.txt";

            // Act
            _fileImplicationRuleProvider.FilePath = expectedFilePath;

            // Assert
            Assert.AreEqual(expectedFilePath, _fileImplicationRuleProvider.FilePath);
        }

        [Test]
        public void FilePath_GetterReturnsValue()
        {
            // Arrange
            string expectedFilePath = "SomeFile.txt";
            _fileImplicationRuleProvider.FilePath = expectedFilePath;

            // Act
            string actualFilePath = _fileImplicationRuleProvider.FilePath;

            // Assert
            Assert.AreEqual(expectedFilePath, actualFilePath);
        }

        [Test]
        public void GetImplicationRules_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _fileImplicationRuleProvider.GetImplicationRules());
        }

        [Test]
        public void GetImplicationRules_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Arrange
            _fileImplicationRuleProvider.FilePath = "";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileImplicationRuleProvider.GetImplicationRules());
        }

        [Test]
        public void GetImplicationRules_ThrowsFileNotFoundExceptionIfFileDoesntExists()
        {
            // Arrange
            _fileImplicationRuleProvider.FilePath = "NotExistingFile.txt";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileImplicationRuleProvider.GetImplicationRules());
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyListOfRules()
        {
            // Arrange
            _fileImplicationRuleProvider.FilePath = _filePath;
            _fileReaderMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(new List<string>());

            // Act
            List<ImplicationRule> expectedImplicationRules = _fileImplicationRuleProvider.GetImplicationRules();

            // Assert
            Assert.IsEmpty(expectedImplicationRules);
        }

        [Test]
        public void GetImplicationRules_ReturnsCorrectListOfRules()
        {
            // Arrange
            _fileImplicationRuleProvider.FilePath = _filePath;

            List<string> implicationRulesFromFile = new List<string>
            {
                "IF (A > 10) THEN (X = 5)",
                "IF (B != 1 & C != 2) THEN (X = 10)"
            };
            _fileReaderMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(implicationRulesFromFile);

            // IF (A > 10) THEN (X = 5)
            ImplicationRuleStrings firstImplicationRuleStrings = new ImplicationRuleStrings("A>10", "X=5");
            _implicationRuleCreatorMock.Stub(x => x.DivideImplicationRule(implicationRulesFromFile[0]))
                .Return(firstImplicationRuleStrings);
            ImplicationRule firstImplicationRule = new ImplicationRule(
            new List<StatementCombination>
            {
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("A", ComparisonOperation.Greater, "10")
                })
            },
            new StatementCombination(new List<UnaryStatement>
            {
                new UnaryStatement("X", ComparisonOperation.Equal, "5")
            }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(firstImplicationRuleStrings))
                .Return(firstImplicationRule);

            // IF (B != 1 & C != 2) THEN (X = 10)
            ImplicationRuleStrings secondImplicationRuleStrings = new ImplicationRuleStrings("B!=1&C!=2", "X=10");
            _implicationRuleCreatorMock.Stub(x => x.DivideImplicationRule(implicationRulesFromFile[1]))
                .Return(secondImplicationRuleStrings);
            ImplicationRule secondImplicationRule = new ImplicationRule(
            new List<StatementCombination>
            {
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("B", ComparisonOperation.NotEqual, "1"),
                    new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                })
            },
            new StatementCombination(new List<UnaryStatement>
            {
                new UnaryStatement("X", ComparisonOperation.Equal, "10")
            }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(secondImplicationRuleStrings))
                .Return(secondImplicationRule);

            List<ImplicationRule> expectedImplicationRules = new List<ImplicationRule>
            {
                firstImplicationRule, secondImplicationRule
            };

            // Act
            List<ImplicationRule> actualImplicationRules = _fileImplicationRuleProvider.GetImplicationRules();

            // Assert
            Assert.AreEqual(expectedImplicationRules, actualImplicationRules);
        }
    }
}
