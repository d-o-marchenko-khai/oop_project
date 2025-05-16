using System;
using System.Collections.Generic;
using Xunit;

namespace oop_project.Tests
{
    public class ChatTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenArgumentsAreValid()
        {
            var advertisementId = Guid.NewGuid();
            var participantIds = new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid());

            var chat = new Chat(advertisementId, participantIds);

            Assert.Equal(advertisementId, chat.AdvertisementId);
            Assert.Equal(participantIds, chat.ParticipantIds);
            Assert.NotEqual(Guid.Empty, chat.Id);
            Assert.True((DateTime.Now - chat.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenParticipantIdsAreInvalid()
        {
            Assert.Throws<ArgumentException>(() => new Chat(Guid.NewGuid(), null));
            Assert.Throws<ArgumentException>(() => new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.Empty, Guid.NewGuid())));
            Assert.Throws<ArgumentException>(() => new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.Empty)));
        }

        [Fact]
        public void AddMessage_ShouldAddMessage_WhenArgumentsAreValid()
        {
            var advertisementId = Guid.NewGuid();
            var participant1 = Guid.NewGuid();
            var participant2 = Guid.NewGuid();
            var chat = new Chat(advertisementId, new Tuple<Guid, Guid>(participant1, participant2));
            var senderId = participant1;
            var text = "Hello, this is a test message.";

            var message = chat.AddMessage(senderId, text);

            Assert.NotNull(message);
            Assert.Equal(chat.Id, message.ChatId);
            Assert.Equal(senderId, message.SenderId);
            Assert.Equal(text, message.Text);
        }

        [Fact]
        public void AddMessage_ShouldThrowArgumentException_WhenSenderIdIsInvalid()
        {
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));

            Assert.Throws<ArgumentException>(() => chat.AddMessage(Guid.Empty, "Test message"));
        }

        [Fact]
        public void AddMessage_ShouldThrowArgumentException_WhenTextIsInvalid()
        {
            var participant1 = Guid.NewGuid();
            var participant2 = Guid.NewGuid();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(participant1, participant2));

            Assert.Throws<ArgumentException>(() => chat.AddMessage(participant1, ""));
            Assert.Throws<ArgumentException>(() => chat.AddMessage(participant1, "   "));
        }

        [Fact]
        public void AddMessage_ShouldThrowInvalidOperationException_WhenSenderIsNotAParticipant()
        {
            var participant1 = Guid.NewGuid();
            var participant2 = Guid.NewGuid();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(participant1, participant2));
            var nonParticipant = Guid.NewGuid();

            Assert.Throws<InvalidOperationException>(() => chat.AddMessage(nonParticipant, "Test message"));
        }

        [Fact]
        public void GetHistory_ShouldReturnAllMessages()
        {
            var participant1 = Guid.NewGuid();
            var participant2 = Guid.NewGuid();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(participant1, participant2));
            chat.AddMessage(participant1, "Message 1");
            chat.AddMessage(participant2, "Message 2");

            var history = chat.GetHistory();

            Assert.Equal(2, history.Count);
            Assert.Equal("Message 1", history[0].Text);
            Assert.Equal("Message 2", history[1].Text);
        }

        [Fact]
        public void GetHistory_ShouldReturnEmptyList_WhenNoMessagesExist()
        {
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));

            var history = chat.GetHistory();

            Assert.Empty(history);
        }
    }
}

