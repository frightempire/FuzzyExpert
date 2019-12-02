using System;
using DataProvider.Entities;
using NUnit.Framework;

namespace DataProvider.UnitTests.Entities
{
    [TestFixture]
    public class InitialDataTests
    {
        private string _name = "Air";
        private double _value = 20;
        private double _confidenceFactor = 0.5;
        private InitialData _initialData;

        [SetUp]
        public void SetUp()
        {
            _initialData = new InitialData(_name, _value, _confidenceFactor);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfNameIsNotValid()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InitialData(string.Empty, _value, _confidenceFactor));
            Assert.Throws<ArgumentNullException>(() => new InitialData(null, _value, _confidenceFactor));
        }

        [Test]
        public void Name_ReturnsExpectedValue()
        {
            // Assert
            Assert.AreEqual(_name, _initialData.Name);
        }

        [Test]
        public void Value_ReturnsExpectedValue()
        {
            // Assert
            Assert.AreEqual(_value, _initialData.Value);
        }

        [Test]
        public void ConfidenceFactor_ReturnsExpectedValue()
        {
            // Assert
            Assert.AreEqual(_confidenceFactor, _initialData.ConfidenceFactor);
        }
    }
}