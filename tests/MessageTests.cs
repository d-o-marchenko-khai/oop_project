using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;
using System.ComponentModel.DataAnnotations;

namespace tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void MSG_VALID_01_CreateMessage_EmptyText_ThrowsValidationException()
        {
            // Act
            var message = new Message
            {
                Text = ""
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void MSG_VALID_02_CreateMessage_TextTooLong_ThrowsValidationException()
        {
            // Arrange
            var longText = new string('a', 1001);

            // Act
            var message = new Message
            {
                Text = longText
            };
        }
    }
}
