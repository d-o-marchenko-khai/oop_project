using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace oop_project.Tests
{
    public class AdvertisementRepositoryTests
    {
        private readonly AdvertisementRepository _repository;

        public AdvertisementRepositoryTests()
        {
            _repository = AdvertisementRepository.Instance;
        }

        [Fact]
        public void Add_ShouldAddAdvertisement()
        {
            // Arrange
            var advertisement = new SellingAdvertisement(
                "Test Ad",
                "Test Description",
                Guid.NewGuid(),
                Guid.NewGuid(),
                100
            );

            // Act
            _repository.Add(advertisement);

            // Assert
            Assert.Contains(advertisement, _repository.GetAll());
        }

        [Fact]
        public void Add_ShouldThrowArgumentNullException_WhenAdvertisementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Fact]
        public void GetAll_ShouldReturnAllAdvertisements()
        {
            // Arrange
            var advertisement1 = new SellingAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), Guid.NewGuid(), 100);
            var advertisement2 = new SellingAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), Guid.NewGuid(), 200);
            _repository.Add(advertisement1);
            _repository.Add(advertisement2);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Contains(advertisement1, result);
            Assert.Contains(advertisement2, result);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectAdvertisement()
        {
            // Arrange
            var advertisement = new SellingAdvertisement("Ad", "Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            // Act
            var result = _repository.GetById(advertisement.Id);

            // Assert
            Assert.Equal(advertisement, result);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenAdvertisementNotFound()
        {
            // Act
            var result = _repository.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetByUserId_ShouldReturnAdvertisementsForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var advertisement1 = new SellingAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), userId, 100);
            var advertisement2 = new SellingAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), userId, 200);
            _repository.Add(advertisement1);
            _repository.Add(advertisement2);

            // Act
            var result = _repository.GetByUserId(userId);

            // Assert
            Assert.Contains(advertisement1, result);
            Assert.Contains(advertisement2, result);
        }

        [Fact]
        public void FindByFilters_ShouldReturnFilteredAdvertisements()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var advertisement = new SellingAdvertisement("Ad", "Description", categoryId, Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            var filter = new AdvertisementFilterDto
            {
                MinPrice = 50,
                MaxPrice = 150,
                CategoryId = categoryId
            };

            // Act
            var result = _repository.FindByFilters(filter);

            // Assert
            Assert.Contains(advertisement, result);
        }

        [Fact]
        public void Update_ShouldUpdateAdvertisement()
        {
            // Arrange
            var advertisement = new SellingAdvertisement("Old Title", "Old Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            var updatedAd = new SellingAdvertisement("New Title", "New Description", advertisement.CategoryId, advertisement.OwnerId, 150)
            {
                Id = advertisement.Id
            };

            // Act
            _repository.Update(updatedAd);

            // Assert
            var result = _repository.GetById(advertisement.Id);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("New Description", result.Description);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenAdvertisementNotFound()
        {
            // Arrange
            var advertisement = new SellingAdvertisement("Ad", "Description", Guid.NewGuid(), Guid.NewGuid(), 100);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(advertisement));
        }

        [Fact]
        public void Delete_ShouldRemoveAdvertisement()
        {
            // Arrange
            var advertisement = new SellingAdvertisement("Ad", "Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            // Act
            _repository.Delete(advertisement.Id);

            // Assert
            Assert.DoesNotContain(advertisement, _repository.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenAdvertisementNotFound()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(Guid.NewGuid()));
        }
    }
}
