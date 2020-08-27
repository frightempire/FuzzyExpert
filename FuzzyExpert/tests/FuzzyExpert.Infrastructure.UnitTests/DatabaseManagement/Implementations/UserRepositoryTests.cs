using System.IO;
using System.Linq;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.DatabaseManagement.Implementations
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private string _databaseFilename = "Inference.db";
        private UserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new UserRepository(new ConnectionStringProvider());
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_databaseFilename))
            {
                File.Delete(_databaseFilename);
            }
        }

        [Test]
        public void Constructor_DoesNotCreateDatabaseFile()
        {
            // Assert
            Assert.IsFalse(File.Exists(_databaseFilename));
        }

        [Test]
        public void SaveUser_GetUserByName_ExpectedBehaviorForOneProfile()
        {
            // Arrange
            var user = new User
            {
                UserName = "user_name",
                Password = "password"
            };

            // Act
            var result = _userRepository.SaveUser(user);
            var userFromDatabase = _userRepository.GetUserByName(user.UserName);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(userFromDatabase.IsPresent);
            Assert.AreEqual(user.UserName, userFromDatabase.Value.UserName);
        }

        [Test]
        public void SaveProfile_GetProfiles_ExpectedBehaviorForMultipleProfiles()
        {
            // Arrange
            var firstUser = new User
            {
                UserName = "user_name_1",
                Password = "password"
            };
            var secondUser = new User
            {
                UserName = "user_name_2",
                Password = "password"
            };

            // Act
            var firstSaveResult = _userRepository.SaveUser(firstUser);
            var secondSaveResult = _userRepository.SaveUser(secondUser);
            var usersFromDatabase = _userRepository.GetUsers();

            // Assert
            Assert.IsTrue(firstSaveResult);
            Assert.IsTrue(secondSaveResult);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(usersFromDatabase.IsPresent);
            Assert.AreEqual(2, usersFromDatabase.Value.ToList().Count);
        }

        [Test]
        public void SaveProfile_AddsAnotherProfile_IfProfileNameIsChanged()
        {
            // Arrange
            var oldUserName = "user_name_old";
            var newUserName = "user_name_new";
            var user = new User
            {
                UserName = oldUserName,
                Password = "password"
            };
            var firstResult = _userRepository.SaveUser(user);
            user.UserName = newUserName;

            // Act
            var secondResult = _userRepository.SaveUser(user);
            var firstUserFromDatabase = _userRepository.GetUserByName(newUserName);
            var secondUserFromDatabase = _userRepository.GetUserByName(oldUserName);

            // Assert
            Assert.IsTrue(firstResult);
            Assert.IsTrue(secondResult);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(firstUserFromDatabase.IsPresent);
            Assert.IsTrue(secondUserFromDatabase.IsPresent);
            Assert.AreEqual(newUserName, firstUserFromDatabase.Value.UserName);
            Assert.AreEqual(oldUserName, secondUserFromDatabase.Value.UserName);
        }

        [Test]
        public void SaveProfile_UpdatesAlreadyExistingProfile()
        {
            // Arrange
            var user = new User
            {
                UserName = "user_name",
                Password = "password"
            };
            var firstResult = _userRepository.SaveUser(user);
            user.Password = "password_new";

            // Act
            var secondResult = _userRepository.SaveUser(user);
            var updatedUserFromDatabase = _userRepository.GetUserByName(user.UserName);
            var usersFromDatabase = _userRepository.GetUsers();

            // Assert
            Assert.IsTrue(firstResult); // LiteDB Upsert returns TRUE if INSERTED
            Assert.IsFalse(secondResult); // LiteDB Upsert returns FALSE if UPDATED
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(updatedUserFromDatabase.IsPresent);
            Assert.IsTrue(usersFromDatabase.IsPresent);
            Assert.AreEqual(1, usersFromDatabase.Value.ToList().Count);
            Assert.AreEqual(user.UserName, updatedUserFromDatabase.Value.UserName);
            Assert.AreEqual(user.Password, updatedUserFromDatabase.Value.Password);
        }
    }
}