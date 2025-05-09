using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System.ComponentModel.DataAnnotations;

namespace tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void U_AUTH_01_Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var user = new User
            {
                Username = "tester1",
                Password = "pass123",
                FirstName = "Ivan",
                LastName = "Ivanov",
                Phone = "+38(067)-1234567"
            };

            // Act
            var result = user.Authenticate("tester1", "pass123");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void U_AUTH_02_Authenticate_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                Username = "tester1",
                Password = "pass123"
            };

            // Act
            var result = user.Authenticate("tester1", "wrong");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void U_VALID_01_CreateUser_EmptyUsername_ThrowsValidationException()
        {
            // Act
            var user = new User
            {
                Username = "",
                Password = "pass123"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void U_VALID_02_CreateUser_ShortPassword_ThrowsValidationException()
        {
            // Act
            var user = new User
            {
                Username = "tester1",
                Password = "short1"
            };
        }
    }
}
