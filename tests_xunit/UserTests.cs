using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace oop_project.Tests
{
    public class UserTests
    {
        private readonly Mock<IAdvertisementRepository> _mockAdvertisementRepository;

        public UserTests()
        {
            _mockAdvertisementRepository = new Mock<IAdvertisementRepository>();
        }

        [Fact]
        public void ViewAdvertisements_ShouldReturnFilteredAdvertisements_WhenFilterIsValid()
        {
            // Arrange
            var filter = new AdvertisementFilterDto
            {
                Type = AdvertisementType.Selling,
                MinPrice = 100,
                MaxPrice = 500
            };

            var expectedAdvertisements = new List<Advertisement>
            {
                new Mock<Advertisement>("Title1", "Description1", Guid.NewGuid(), Guid.NewGuid()).Object,
                new Mock<Advertisement>("Title2", "Description2", Guid.NewGuid(), Guid.NewGuid()).Object
            };

            _mockAdvertisementRepository
                .Setup(repo => repo.FindByFilters(filter))
                .Returns(expectedAdvertisements);

            var user = new MockUser(_mockAdvertisementRepository.Object);

            // Act
            var result = user.ViewAdvertisements(filter);

            // Assert
            Assert.Equal(expectedAdvertisements, result);
        }

        [Fact]
        public void ViewAdvertisements_ShouldThrowArgumentNullException_WhenFilterIsNull()
        {
            // Arrange
            var user = new MockUser(_mockAdvertisementRepository.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => user.ViewAdvertisements(null));
        }

        private class MockUser : User
        {
            public MockUser(IAdvertisementRepository advertisementRepository) 
                : base(advertisementRepository)
            {
            }
        }
    }
}
