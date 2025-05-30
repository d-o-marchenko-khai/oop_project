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
            AdvertisementRepository.Instance = _mockAdvertisementRepository.Object;
        }

        [Fact]
        public void ViewAdvertisements_ShouldReturnFilteredAdvertisements_WhenFilterIsValid()
        {
            // Arrange
            var filter = new AdvertisementFilterDto
            {
                Type = AdvertisementType.Selling,
                CategoryId = Guid.NewGuid(),
                MinPrice = 100,
                MaxPrice = 500
            };

            var expectedAdvertisements = new List<Advertisement>
            {
                new MockAdvertisement("Title1", "Description1", Guid.NewGuid(), Guid.NewGuid()),
                new MockAdvertisement("Title2", "Description2", Guid.NewGuid(), Guid.NewGuid())
            };

            _mockAdvertisementRepository
                .Setup(repo => repo.FindByFilters(filter))
                .Returns(expectedAdvertisements);

            var user = new MockUser();

            // Act
            var result = user.ViewAdvertisements(filter);

            // Assert
            Assert.Equal(expectedAdvertisements, result);
        }

        [Fact]
        public void ViewAdvertisements_ShouldThrowArgumentNullException_WhenFilterIsNull()
        {
            // Arrange
            var user = new MockUser();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => user.ViewAdvertisements(null));
        }

        private class MockUser : User
        {
            // Mock implementation of the abstract User class
        }

        private class MockAdvertisement : Advertisement
        {
            public MockAdvertisement(string title, string description, Guid categoryId, Guid ownerId)
                : base(title, description, categoryId, ownerId)
            {
            }

            public override bool Publish() => true;
            public override bool Unpublish() => true;
            public override int CompareTo(Advertisement other) => 0;
            public override bool Promote() => true;
            public override Advertisement Update(Advertisement advertisement) => this;
        }
    }
}
