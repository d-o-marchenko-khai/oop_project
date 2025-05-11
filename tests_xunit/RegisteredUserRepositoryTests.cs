using System;
using System.Collections.Generic;
using Xunit;

using System;
using System.Collections.Generic;
using Xunit;

namespace oop_project.Tests
{
    public class RegisteredUserRepositoryTests
    {
        private readonly IRegisteredUserRepository _repository;

        public RegisteredUserRepositoryTests()
        {
            _repository = RegisteredUserRepository.Instance;
            _repository.GetAll().Clear(); // Ensure a clean state before each test
        }

        [Fact]
        public void Add_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var user = CreateTestUser("testuser1");

            // Act
            _repository.Add(user);

            // Assert
            Assert.Contains(user, _repository.GetAll());
        }

        [Fact]
        public void Add_ShouldThrowArgumentNullException_WhenUserIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Fact]
        public void Add_ShouldThrowInvalidOperationException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var user1 = CreateTestUser("duplicateuser");
            var user2 = CreateTestUser("duplicateuser");
            _repository.Add(user1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _repository.Add(user2));
        }

        [Fact]
        public void GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var user1 = CreateTestUser("___user1");
            var user2 = CreateTestUser("___user2");
            _repository.Add(user1);
            _repository.Add(user2);

            // Act
            var users = _repository.GetAll();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.Contains(user1, users);
            Assert.Contains(user2, users);
        }

        [Fact]
        public void GetById_ShouldReturnUser_WhenIdExists()
        {
            // Arrange
            var user = CreateTestUser("testuser");
            _repository.Add(user);

            // Act
            var result = _repository.GetById(user.Id);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Act
            var result = _repository.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetByUsername_ShouldReturnUser_WhenUsernameExists()
        {
            // Arrange
            var user = CreateTestUser("existinguser");
            _repository.Add(user);

            // Act
            var result = _repository.GetByUsername("existinguser");

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void GetByUsername_ShouldThrowInvalidOperationException_WhenUsernameDoesNotExist()
        {
            // Act & Assert
            Assert.Equal(null, _repository.GetByUsername("nonexistentuser"));
        }

        [Fact]
        public void Update_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var user = CreateTestUser("updatableuser");
            _repository.Add(user);
            var updatedUser = new RegisteredUser(
                new InMemoryAdvertisementRepository(),
                new InMemoryChatRepository(),
                RegisteredUserRepository.Instance,
                new RegisterUserDto
                {
                    Username = "updateduser",
                    Password = "newpassword1",
                    FirstName = "Updated",
                    LastName = "User",
                    Phone = "1234567890"
                }
            )
            { Id = user.Id };

            // Act
            _repository.Update(updatedUser);

            // Assert
            var result = _repository.GetById(user.Id);
            Assert.Equal("updateduser", result.Username);
            Assert.Equal("newpassword1", result.Password);
            Assert.Equal("Updated", result.FirstName);
            Assert.Equal("User", result.LastName);
            Assert.Equal("1234567890", result.Phone);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var user = CreateTestUser("nonexistentuser");

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(user));
        }

        [Fact]
        public void Delete_ShouldRemoveUser_WhenIdExists()
        {
            // Arrange
            var user = CreateTestUser("deletableuser");
            _repository.Add(user);

            // Act
            _repository.Delete(user.Id);

            // Assert
            Assert.DoesNotContain(user, _repository.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenIdDoesNotExist()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(Guid.NewGuid()));
        }

        private RegisteredUser CreateTestUser(string username)
        {
            var advertisementRepository = new InMemoryAdvertisementRepository();
            var chatRepository = new InMemoryChatRepository();
            return new RegisteredUser(
                advertisementRepository,
                chatRepository,
                RegisteredUserRepository.Instance,
                new RegisterUserDto
                {
                    Username = username,
                    Password = "password1",
                    FirstName = "Test",
                    LastName = "User",
                    Phone = "1234567890"
                }
            );
        }
    }

    // Mock implementations for testing
    public class InMemoryAdvertisementRepository : IAdvertisementRepository
    {
        private readonly List<Advertisement> _advertisements = new();

        public void Add(Advertisement advertisement) => _advertisements.Add(advertisement);
        public List<Advertisement> GetAll() => _advertisements;
        public Advertisement GetById(Guid id) => _advertisements.FirstOrDefault(ad => ad.Id == id);
        public List<Advertisement> GetByUserId(Guid userId) => _advertisements.Where(ad => ad.OwnerId == userId).ToList();
        public List<Advertisement> FindByFilters(AdvertisementFilterDto filter) => _advertisements;
        public void Update(Advertisement advertisement) { }
        public void Delete(Guid id) => _advertisements.RemoveAll(ad => ad.Id == id);
    }

    public class InMemoryChatRepository : IChatRepository
    {
        private readonly List<Chat> _chats = new();

        public void Add(Chat chat) => _chats.Add(chat);
        public List<Chat> GetAll() => _chats;
        public Chat GetById(Guid id) => _chats.FirstOrDefault(chat => chat.Id == id);
        public List<Chat> GetByParticipant(Guid participantId) => _chats.Where(chat => chat.ParticipantIds.Item1 == participantId || chat.ParticipantIds.Item2 == participantId).ToList();
        public void Update(Chat chat) { }
        public void Delete(Guid id) => _chats.RemoveAll(chat => chat.Id == id);
    }
}
