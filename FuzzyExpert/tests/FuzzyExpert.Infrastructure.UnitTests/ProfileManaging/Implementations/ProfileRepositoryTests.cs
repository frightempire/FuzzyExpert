using System.Collections.Generic;
using System.IO;
using System.Linq;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.ProfileManaging.Implementations
{
    [TestFixture]
    public class ProfileRepositoryTests
    {
        private string _databaseFilename = "Inference.db";
        private ProfileRepository _profileRepository;

        [SetUp]
        public void SetUp()
        {
            _profileRepository = new ProfileRepository();
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
        public void SaveProfile_GetProfileByName_ExpectedBehaviorForOneProfile()
        {
            // Arrange
            var profile = new InferenceProfile
            {
                ProfileName = "ProjectSelection",
                Rules = new List<string>
                {
                    "IF A=1 THEN B=2",
                    "IF B=2 THEN C=3"
                },
                Variables = new List<string>
                {
                    "A", "B", "C"
                },
                Functions = new List<string>
                {
                    "A:Initial:[1|2|3]",
                    "B:Derivative:[1|2|3]",
                    "C:Derivative:[1|2|3]"
                }
            };

            // Act
            var result = _profileRepository.SaveProfile(profile);
            var profileFromDatabase = _profileRepository.GetProfileByName(profile.ProfileName);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(profileFromDatabase.IsPresent);
            Assert.AreEqual(profile.ProfileName, profileFromDatabase.Value.ProfileName);
            Assert.AreEqual(2, profileFromDatabase.Value.Rules.Count);
            Assert.AreEqual(3, profileFromDatabase.Value.Variables.Count);
            Assert.AreEqual(3, profileFromDatabase.Value.Functions.Count);
        }

        [Test]
        public void SaveProfile_GetProfiles_ExpectedBehaviorForMultipleProfiles()
        {
            // Arrange
            var firstProfile = new InferenceProfile
            {
                ProfileName = "ProjectSelection",
                Rules = new List<string>
                {
                    "IF A=1 THEN B=2",
                    "IF B=2 THEN C=3"
                },
                Variables = new List<string>
                {
                    "A", "B", "C"
                },
                Functions = new List<string>
                {
                    "A:Initial:[1|2|3]",
                    "B:Derivative:[1|2|3]",
                    "C:Derivative:[1|2|3]"
                }
            };
            var secondProfile = new InferenceProfile
            {
                ProfileName = "BusinessCaseCreation",
                Rules = new List<string>
                {
                    "IF A=1 THEN B=2",
                    "IF B=2 THEN C=3"
                },
                Variables = new List<string>
                {
                    "A", "B", "C"
                },
                Functions = new List<string>
                {
                    "A:Initial:[1|2|3]",
                    "B:Derivative:[1|2|3]",
                    "C:Derivative:[1|2|3]"
                }
            };

            // Act
            var firstSaveResult = _profileRepository.SaveProfile(firstProfile);
            var secondSaveResult = _profileRepository.SaveProfile(secondProfile);
            var profilesFromDatabase = _profileRepository.GetProfiles();

            // Assert
            Assert.IsTrue(firstSaveResult);
            Assert.IsTrue(secondSaveResult);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(profilesFromDatabase.IsPresent);
            Assert.AreEqual(2, profilesFromDatabase.Value.ToList().Count);
            Assert.AreEqual(firstProfile.ProfileName, profilesFromDatabase.Value.ToList()[1].ProfileName);
            Assert.AreEqual(secondProfile.ProfileName, profilesFromDatabase.Value.ToList()[0].ProfileName);
        }

        [Test]
        public void SaveProfile_AddsAnotherProfile_IfProfileNameIsChanged()
        {
            // Arrange
            string oldProfileName = "ProjectSelection";
            string newProfileName = "ProjectSelection#2";
            var profile = new InferenceProfile
            {
                ProfileName = oldProfileName,
                Rules = new List<string>
                {
                    "IF A=1 THEN B=2",
                    "IF B=2 THEN C=3"
                },
                Variables = new List<string>
                {
                    "A", "B", "C"
                },
                Functions = new List<string>
                {
                    "A:Initial:[1|2|3]",
                    "B:Derivative:[1|2|3]",
                    "C:Derivative:[1|2|3]"
                }
            };
            var firstResult = _profileRepository.SaveProfile(profile);
            profile.ProfileName = newProfileName;

            // Act
            var secondResult = _profileRepository.SaveProfile(profile);
            var firstProfileFromDatabase = _profileRepository.GetProfileByName(newProfileName);
            var secondProfileFromDatabase = _profileRepository.GetProfileByName(oldProfileName);

            // Assert
            Assert.IsTrue(firstResult);
            Assert.IsTrue(secondResult);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(firstProfileFromDatabase.IsPresent);
            Assert.IsTrue(secondProfileFromDatabase.IsPresent);
            Assert.AreEqual(newProfileName, firstProfileFromDatabase.Value.ProfileName);
            Assert.AreEqual(oldProfileName, secondProfileFromDatabase.Value.ProfileName);
        }

        [Test]
        public void SaveProfile_UpdatesAlreadyExistingProfile()
        {
            // Arrange
            var profile = new InferenceProfile
            {
                ProfileName = "ProjectSelection",
                Rules = new List<string>
                {
                    "IF A=1 THEN B=2",
                    "IF B=2 THEN C=3"
                },
                Variables = new List<string>
                {
                    "A", "B", "C"
                },
                Functions = new List<string>
                {
                    "A:Initial:[1|2|3]",
                    "B:Derivative:[1|2|3]",
                    "C:Derivative:[1|2|3]"
                }
            };
            var firstResult = _profileRepository.SaveProfile(profile);
            profile.Rules.Add("IF C=3 THEN D=4");

            // Act
            var secondResult = _profileRepository.SaveProfile(profile);
            var updatedProfileFromDatabase = _profileRepository.GetProfileByName(profile.ProfileName);
            var profilesFromDatabase = _profileRepository.GetProfiles();

            // Assert
            Assert.IsTrue(firstResult); // LiteDB Upsert returns TRUE if INSERTED
            Assert.IsFalse(secondResult); // LiteDB Upsert returns FALSE if UPDATED
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.IsTrue(updatedProfileFromDatabase.IsPresent);
            Assert.IsTrue(profilesFromDatabase.IsPresent);
            Assert.AreEqual(1, profilesFromDatabase.Value.ToList().Count);
            Assert.AreEqual(profile.ProfileName, updatedProfileFromDatabase.Value.ProfileName);
            Assert.AreEqual(3, updatedProfileFromDatabase.Value.Rules.Count);
        }

        [Test]
        public void DeleteProfile_DeletesCreatedProfile()
        {
            // Arrange
            var profile = new InferenceProfile
            {
                ProfileName = "ProjectSelection",
                Rules = new List<string>
                {
                    "IF A=1 THEN B=2",
                    "IF B=2 THEN C=3"
                },
                Variables = new List<string>
                {
                    "A", "B", "C"
                },
                Functions = new List<string>
                {
                    "A:Initial:[1|2|3]",
                    "B:Derivative:[1|2|3]",
                    "C:Derivative:[1|2|3]"
                }
            };

            // Act
            var result = _profileRepository.SaveProfile(profile);
            var entitiesDeletedCount = _profileRepository.DeleteProfile(profile.ProfileName);
            var notExistingProfile = _profileRepository.GetProfileByName(profile.ProfileName);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_databaseFilename));
            Assert.AreEqual(1, entitiesDeletedCount);
            Assert.IsFalse(notExistingProfile.IsPresent);
        }
    }
}