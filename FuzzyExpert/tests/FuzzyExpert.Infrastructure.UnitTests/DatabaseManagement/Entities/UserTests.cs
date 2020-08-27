using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.DatabaseManagement.Entities
{
    [TestFixture]
    public class UserTests
    {
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _user = new User();
        }

        [Test]
        public void UserName_PropertyExpectedBehavior()
        {
            // Arrange
            var expectedUserName = "user_name";

            // Act
            _user.UserName = expectedUserName;

            // Assert
            Assert.AreEqual(expectedUserName, _user.UserName);
        }

        [Test]
        public void Password_PropertyExpectedBehavior()
        {
            // Arrange
            var expectedPassword = "password";

            // Act
            _user.Password = expectedPassword;

            // Assert
            Assert.AreEqual(expectedPassword, _user.Password);
        }
    }
}