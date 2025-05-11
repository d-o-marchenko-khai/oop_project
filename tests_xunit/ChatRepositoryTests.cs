using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace oop_project.Tests
{
    public class ChatRepositoryTests
    {
        [Fact]
        public void Add_ShouldAddChat_WhenChatIsValid()
        {
            // Arrange
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));
            ChatRepository.Instance.GetAll().Clear(); // Ensure a clean state

            // Act
            ChatRepository.Instance.Add(chat);

            // Assert
            Assert.Contains(chat, ChatRepository.Instance.GetAll());
        }

        [Fact]
        public void Add_ShouldThrowArgumentNullException_WhenChatIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ChatRepository.Instance.Add(null));
        }

        [Fact]
        public void GetAll_ShouldReturnAllChats()
        {
            // Arrange
            var chat1 = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));
            var chat2 = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));
            ChatRepository.Instance.GetAll().Clear();
            ChatRepository.Instance.Add(chat1);
            ChatRepository.Instance.Add(chat2);

            // Act
            var chats = ChatRepository.Instance.GetAll();

            // Assert
            Assert.Equal(2, chats.Count);
            Assert.Contains(chat1, chats);
            Assert.Contains(chat2, chats);
        }

        [Fact]
        public void GetById_ShouldReturnChat_WhenIdExists()
        {
            // Arrange
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));
            ChatRepository.Instance.GetAll().Clear();
            ChatRepository.Instance.Add(chat);

            // Act
            var result = ChatRepository.Instance.GetById(chat.Id);

            // Assert
            Assert.Equal(chat, result);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Act
            var result = ChatRepository.Instance.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetByParticipant_ShouldReturnChats_WhenParticipantExists()
        {
            // Arrange
            var participantId = Guid.NewGuid();
            var chat1 = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(participantId, Guid.NewGuid()));
            var chat2 = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), participantId));
            ChatRepository.Instance.GetAll().Clear();
            ChatRepository.Instance.Add(chat1);
            ChatRepository.Instance.Add(chat2);

            // Act
            var result = ChatRepository.Instance.GetByParticipant(participantId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(chat1, result);
            Assert.Contains(chat2, result);
        }

        [Fact]
        public void GetByParticipant_ShouldReturnEmptyList_WhenParticipantDoesNotExist()
        {
            // Act
            var result = ChatRepository.Instance.GetByParticipant(Guid.NewGuid());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Update_ShouldUpdateChat_WhenChatExists()
        {
            // Arrange
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));
            ChatRepository.Instance.GetAll().Clear();
            ChatRepository.Instance.Add(chat);
            var updatedChat = new Chat(chat.AdvertisementId, new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid())) { Id = chat.Id };

            // Act
            ChatRepository.Instance.Update(updatedChat);

            // Assert
            var result = ChatRepository.Instance.GetById(chat.Id);
            Assert.Equal(updatedChat.ParticipantIds, result.ParticipantIds);
            Assert.Equal(updatedChat.AdvertisementId, result.AdvertisementId);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenChatDoesNotExist()
        {
            // Arrange
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => ChatRepository.Instance.Update(chat));
        }

        [Fact]
        public void Delete_ShouldRemoveChat_WhenIdExists()
        {
            // Arrange
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid()));
            ChatRepository.Instance.GetAll().Clear();
            ChatRepository.Instance.Add(chat);

            // Act
            ChatRepository.Instance.Delete(chat.Id);

            // Assert
            Assert.DoesNotContain(chat, ChatRepository.Instance.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenIdDoesNotExist()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => ChatRepository.Instance.Delete(Guid.NewGuid()));
        }
    }
}
