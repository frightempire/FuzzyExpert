using System;
using Base.UnitTests.TestEntitites;
using CommonLogic.Extensions;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Extensions
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetDescription_ThrowsArgumentExceptionIfPassedValueIsNotEnum()
        {
            // Arrange
            TestStruct testStruct = new TestStruct();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { testStruct.GetDescription(); });
        }

        [Test]
        public void GetDescription_ReturnsCorrectDescriptionCase1()
        {
            // Arrange
            TestEnum enumValue = TestEnum.Zero;
            string expectedDescription = "Nothing";

            // Act
            string actualDescription = enumValue.GetDescription();

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void GetDescription_ReturnsCorrectDescriptionCase2()
        {
            // Arrange
            TestEnum enumValue = TestEnum.First;
            string expectedDescription = "Something";

            // Act
            string actualDescription = enumValue.GetDescription();

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }
    }
}
