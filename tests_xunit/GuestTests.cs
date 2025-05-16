using System;
using Xunit;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace oop_project.Tests
{
    public class GuestTests
    {
        private readonly Mock<IAdvertisementRepository> _mockAdvertisementRepository;
        private readonly Mock<IChatRepository> _mockChatRepository;
        private readonly Mock<IRegisteredUserRepository> _mockRegisteredUserRepository;

        public GuestTests()
        {
            _mockAdvertisementRepository = new Mock<IAdvertisementRepository>();
            _mockChatRepository = new Mock<IChatRepository>();
            _mockRegisteredUserRepository = new Mock<IRegisteredUserRepository>();
        }

        private Guest CreateGuest()
        {
            return new Guest(
                _mockAdvertisementRepository.Object,
                _mockChatRepository.Object,
                _mockRegisteredUserRepository.Object
            );
        }

        [Fact]
        public void Register_ShouldAddRegisteredUser_WhenDtoIsValid()
        {
            var guest = CreateGuest();
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Password = "password1",
                FirstName = "Test",
                LastName = "User",
                Phone = "1234567890"
            };

            var registeredUser = guest.Register(dto);

            _mockRegisteredUserRepository.Verify(repo => repo.Add(It.IsAny<RegisteredUser>()), Times.Once);
            Assert.NotNull(registeredUser);
            Assert.Equal(dto.Username, registeredUser.Username);
            Assert.Equal(dto.Password, registeredUser.Password);
            Assert.Equal(dto.FirstName, registeredUser.FirstName);
            Assert.Equal(dto.LastName, registeredUser.LastName);
            Assert.Equal(dto.Phone, registeredUser.Phone);
        }

        [Fact]
        public void Register_ShouldThrowException_WhenDtoIsInvalid()
        {
            var guest = CreateGuest();
            var dto = new RegisterUserDto
            {
                Username = "",
                Password = "password1",
                FirstName = "Test",
                LastName = "User",
                Phone = "1234567890"
            };

            Assert.Throws<ValidationException>(() => guest.Register(dto));
        }
    }
}
