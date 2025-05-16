using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace oop_project.Tests
{
    public class MessageTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenArgumentsAreValid()
        {
            var chatId = Guid.NewGuid();
            var senderId = Guid.NewGuid();
            var text = "This is a test message.";

            var message = new Message(chatId, senderId, text);

            Assert.Equal(chatId, message.ChatId);
            Assert.Equal(senderId, message.SenderId);
            Assert.Equal(text, message.Text);
            Assert.NotEqual(Guid.Empty, message.Id);
            Assert.True((DateTime.Now - message.SentAt).TotalSeconds < 1);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenChatIdIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new Message(Guid.Empty, Guid.NewGuid(), "Test message"));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenSenderIdIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new Message(Guid.NewGuid(), Guid.Empty, "Test message"));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenTextIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new Message(Guid.NewGuid(), Guid.NewGuid(), ""));
            Assert.Throws<ArgumentException>(() => new Message(Guid.NewGuid(), Guid.NewGuid(), "   "));
        }

        [Fact]
        public void Text_Setter_ShouldUpdateText_WhenTextIsValid()
        {
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), "Initial text");

            message.Text = "Updated text";

            Assert.Equal("Updated text", message.Text);
        }

        [Fact]
        public void Text_Setter_ShouldThrowArgumentException_WhenTextIsInvalid()
        {
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), "Initial text");

            Assert.Throws<ArgumentException>(() => message.Text = "");
            Assert.Throws<ArgumentException>(() => message.Text = "   ");
        }
    }
}

