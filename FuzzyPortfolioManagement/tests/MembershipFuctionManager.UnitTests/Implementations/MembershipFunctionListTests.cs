using System;
using MembershipFuctionManager.UnitTests.TestEntities;
using MembershipFunctionManager.Entities;
using MembershipFunctionManager.Implementations;
using NUnit.Framework;

namespace MembershipFuctionManager.UnitTests.Implementations
{
    [TestFixture]
    public class MembershipFunctionListTests
    {
        private MembershipFunctionList _membershipFunctionList;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionList = new MembershipFunctionList
            {
                new StubMembershipFunction("FunctionNr1"),
                new StubMembershipFunction("FuctionnNr2"),
                new StubMembershipFunction("FunctionNr3")
            };
        }

        [Test]
        public void FindByVariableName_ThrowsArgumentNullExceptionIfThereAreNoMembershipFunctionsInList()
        {
            // Arrange
            _membershipFunctionList = new MembershipFunctionList();
            string variableName = "AnyName";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { _membershipFunctionList.FindByVariableName(variableName); });
        }

        [Test]
        public void FindByVariableName_ThrowsArgumentExceptionIfThereIsNoMembershipFunctionForVariable()
        {
            // Arrange
            string variableName = "FunctionNr4";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _membershipFunctionList.FindByVariableName(variableName); });
        }

        [Test]
        public void FindByVariableName_ThrowsArgumentOutOfRangeExceptionIfThereIsAreMoreThanOneMembershipFunctionsForVariable()
        {
            // Arrange
            _membershipFunctionList.Add(new StubMembershipFunction("FunctionNr1"));
            string variableName = "FunctionNr1";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { _membershipFunctionList.FindByVariableName(variableName); });
        }

        [Test]
        public void FindByVariableName_ReturnsMembershipFunctionForVariable()
        {
            // Arrange
            string variableName = "FunctionNr1";

            // Act
            MembershipFunction actualMembershipFunction = _membershipFunctionList.FindByVariableName(variableName);

            // Assert
            Assert.AreEqual(variableName, actualMembershipFunction.LinguisticVariableName);
        }
    }
}