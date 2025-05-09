using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace tests
{
    [TestClass]
    public class ChatTests
    {
        [TestMethod]
        public void CHAT_ADD_01_AddMessage_ValidParticipant_AddsMessage()
        {
            // Arrange
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(userA, userB));

            // Act
            var message = chat.AddMessage(userA, "Hi");

            // Assert
            Assert.IsNotNull(message);
            Assert.AreEqual(userA, message.SenderId);
            Assert.AreEqual("Hi", message.Text);
            Assert.IsTrue((DateTime.Now - message.SentAt).TotalSeconds < 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CHAT_ADD_02_AddMessage_InvalidParticipant_ThrowsException()
        {
            // Arrange
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();
            var userC = Guid.NewGuid();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(userA, userB));

            // Act
            chat.AddMessage(userC, "Hello");
        }

        [TestMethod]
        public void CHAT_HIST_01_GetHistory_ReturnsMessagesSortedBySentAt()
        {
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            // Arrange
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(userA, userB));
            var message1 = new Message { SentAt = DateTime.Now.AddMinutes(-10) };
            var message2 = new Message { SentAt = DateTime.Now.AddMinutes(-5) };
            var message3 = new Message { SentAt = DateTime.Now.AddMinutes(-1) };

            // Simulate adding messages to the chat
            chat.GetType().GetProperty("Messages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(chat, new List<Message> { message3, message1, message2 });

            // Act
            var history = chat.GetHistory();

            // Assert
            Assert.AreEqual(3, history.Count);
            Assert.AreEqual(message1, history[0]);
            Assert.AreEqual(message2, history[1]);
            Assert.AreEqual(message3, history[2]);
        }
    }
}
