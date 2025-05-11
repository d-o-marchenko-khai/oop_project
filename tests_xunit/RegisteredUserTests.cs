using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using System.Reflection;

namespace oop_project.Tests
{
    public class RegisteredUserTests
    {
        private readonly Mock<IAdvertisementRepository> _mockAdvertisementRepository;
        private readonly Mock<IChatRepository> _mockChatRepository;
        private readonly Mock<IRegisteredUserRepository> _mockRegisteredUserRepository;

        public RegisteredUserTests()
        {
            _mockAdvertisementRepository = new Mock<IAdvertisementRepository>();
            _mockChatRepository = new Mock<IChatRepository>();
            _mockRegisteredUserRepository = new Mock<IRegisteredUserRepository>();
        }

        private RegisteredUser CreateTestUser(string username)
        {
            var dto = new RegisterUserDto
            {
                Username = username,
                Password = "password1",
                FirstName = "Test",
                LastName = "User",
                Phone = "1234567890"
            };

            return new RegisteredUser(
                _mockAdvertisementRepository.Object,
                _mockChatRepository.Object,
                _mockRegisteredUserRepository.Object,
                dto
            );
        }

        [Fact]
        public void Authenticate_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act
            var result = user.Authenticate("testuser", "password1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Authenticate_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act
            var result = user.Authenticate("testuser", "wrongpassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateAdvertisement_ShouldAddAdvertisement_WhenDtoIsValid()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var dto = new CreateAdvertisementDto
            {
                Title = "Test Ad",
                Description = "Test Description",
                CategoryId = Guid.NewGuid(),
                Type = AdvertisementType.Selling,
                Price = 100
            };

            // Act
            var advertisement = user.CreateAdvertisement(dto);

            // Assert
            _mockAdvertisementRepository.Verify(repo => repo.Add(It.IsAny<SellingAdvertisement>()), Times.Once);
            Assert.Equal("Test Ad", advertisement.Title);
            Assert.Equal("Test Description", advertisement.Description);
            Assert.Equal(dto.CategoryId, advertisement.CategoryId);
            Assert.Equal(user.Id, advertisement.OwnerId);
        }

        [Fact]
        public void SendMessage_ShouldAddMessage_WhenChatExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(user.Id, Guid.NewGuid()));
            _mockChatRepository.Setup(repo => repo.GetByParticipant(user.Id)).Returns(new List<Chat> { chat });

            // Act
            var message = user.SendMessage(chat.Id, "Hello!");

            // Assert
            Assert.NotNull(message);
            Assert.Equal("Hello!", message.Text);
            Assert.Equal(user.Id, message.SenderId);
        }

        [Fact]
        public void SendMessage_ShouldThrowException_WhenChatDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.SendMessage(Guid.NewGuid(), "Hello!"));
        }

        [Fact]
        public void GetChatHistory_ShouldReturnMessages_WhenChatExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var chatId = Guid.NewGuid();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(user.Id, Guid.NewGuid()));
            _mockChatRepository.Setup(repo => repo.GetByParticipant(user.Id)).Returns(new List<Chat> { chat });
            var messages = new List<Message>
            {
                new Message(chatId, user.Id, "Message 1"),
                new Message(chatId, user.Id, "Message 2")
            };
            chat.GetType()
                .GetProperty("Messages", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(chat, messages);

            // Act
            var history = user.GetChatHistory(chat.Id);

            // Assert
            Assert.Equal(2, history.Count);
            Assert.Equal("Message 1", history[0].Text);
            Assert.Equal("Message 2", history[1].Text);
        }

        [Fact]
        public void GetChatHistory_ShouldThrowException_WhenChatDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.GetChatHistory(Guid.NewGuid()));
        }

        [Fact]
        public void ContactAdvertisementOwner_ShouldCreateChat_WhenAdvertisementExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var advertisementId = Guid.NewGuid();
            var advertisement = new SellingAdvertisement("Ad Title", "Ad Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _mockAdvertisementRepository.Setup(repo => repo.GetById(advertisementId)).Returns(advertisement);

            // Act
            var chat = user.ContactAdvertisementOwner(advertisementId);

            // Assert
            _mockChatRepository.Verify(repo => repo.Add(It.IsAny<Chat>()), Times.Once);
            Assert.NotNull(chat);
            Assert.Equal(advertisementId, chat.AdvertisementId);
        }

        [Fact]
        public void ContactAdvertisementOwner_ShouldThrowException_WhenAdvertisementDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.ContactAdvertisementOwner(Guid.NewGuid()));
        }

        [Fact]
        public void PromoteAdvertisement_ShouldPromote_WhenAdvertisementExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var advertisement = new SellingAdvertisement("Ad Title", "Ad Description", Guid.NewGuid(), user.Id, 100);
            _mockAdvertisementRepository.Setup(repo => repo.GetByUserId(user.Id)).Returns(new List<Advertisement> { advertisement });

            // Act
            var result = user.PromoteAdvertisement(advertisement.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void PromoteAdvertisement_ShouldThrowException_WhenAdvertisementDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.PromoteAdvertisement(Guid.NewGuid()));
        }

        [Fact]
        public void UpdateAdvertisement_ShouldUpdate_WhenAdvertisementExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var advertisement = new SellingAdvertisement("Ad Title", "Ad Description", Guid.NewGuid(), user.Id, 100);
            _mockAdvertisementRepository.Setup(repo => repo.GetByUserId(user.Id)).Returns(new List<Advertisement> { advertisement });

            // Act
            var result = user.UpdateAdvertisement(advertisement.Id, advertisement);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateAdvertisement_ShouldThrowException_WhenAdvertisementDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.UpdateAdvertisement(Guid.NewGuid(), null));
        }

        [Fact]
        public void DeleteAdvertisement_ShouldDelete_WhenAdvertisementExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            var advertisement = new SellingAdvertisement("Ad Title", "Ad Description", Guid.NewGuid(), user.Id, 100);
            _mockAdvertisementRepository.Setup(repo => repo.GetByUserId(user.Id)).Returns(new List<Advertisement> { advertisement });

            // Act
            var result = user.DeleteAdvertisement(advertisement.Id);

            // Assert
            Assert.True(result);
            _mockAdvertisementRepository.Verify(repo => repo.Delete(advertisement.Id), Times.Once);
        }

        [Fact]
        public void DeleteAdvertisement_ShouldThrowException_WhenAdvertisementDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("testuser");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.DeleteAdvertisement(Guid.NewGuid()));
        }
    }
}
