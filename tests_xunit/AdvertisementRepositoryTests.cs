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
            var advertisement = new SellingAdvertisement(
                "Test Ad",
                "Test Description",
                Guid.NewGuid(),
                Guid.NewGuid(),
                100
            );

            _repository.Add(advertisement);

            Assert.Contains(advertisement, _repository.GetAll());
        }

        [Fact]
        public void Add_ShouldThrowArgumentNullException_WhenAdvertisementIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Fact]
        public void GetAll_ShouldReturnAllAdvertisements()
        {
            var advertisement1 = new SellingAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), Guid.NewGuid(), 100);
            var advertisement2 = new SellingAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), Guid.NewGuid(), 200);
            _repository.Add(advertisement1);
            _repository.Add(advertisement2);

            var result = _repository.GetAll();

            Assert.Contains(advertisement1, result);
            Assert.Contains(advertisement2, result);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectAdvertisement()
        {
            var advertisement = new SellingAdvertisement("Ad", "Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            var result = _repository.GetById(advertisement.Id);

            Assert.Equal(advertisement, result);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenAdvertisementNotFound()
        {
            var result = _repository.GetById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public void GetByUserId_ShouldReturnAdvertisementsForUser()
        {
            var userId = Guid.NewGuid();
            var advertisement1 = new SellingAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), userId, 100);
            var advertisement2 = new SellingAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), userId, 200);
            _repository.Add(advertisement1);
            _repository.Add(advertisement2);

            var result = _repository.GetByUserId(userId);

            Assert.Contains(advertisement1, result);
            Assert.Contains(advertisement2, result);
        }

        [Fact]
        public void FindByFilters_ShouldReturnFilteredAdvertisements()
        {
            var categoryId = Guid.NewGuid();
            var advertisement = new SellingAdvertisement("Ad", "Description", categoryId, Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            var filter = new AdvertisementFilterDto
            {
                MinPrice = 50,
                MaxPrice = 150,
                CategoryId = categoryId
            };

            var result = _repository.FindByFilters(filter);

            Assert.Contains(advertisement, result);
        }

        [Fact]
        public void Update_ShouldUpdateAdvertisement()
        {
            var advertisement = new SellingAdvertisement("Old Title", "Old Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            var updatedAd = new SellingAdvertisement("New Title", "New Description", advertisement.CategoryId, advertisement.OwnerId, 150)
            {
                Id = advertisement.Id
            };

            _repository.Update(updatedAd);

            var result = _repository.GetById(advertisement.Id);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("New Description", result.Description);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenAdvertisementNotFound()
        {
            var advertisement = new SellingAdvertisement("Ad", "Description", Guid.NewGuid(), Guid.NewGuid(), 100);

            Assert.Throws<KeyNotFoundException>(() => _repository.Update(advertisement));
        }

        [Fact]
        public void Delete_ShouldRemoveAdvertisement()
        {
            var advertisement = new SellingAdvertisement("Ad", "Description", Guid.NewGuid(), Guid.NewGuid(), 100);
            _repository.Add(advertisement);

            _repository.Delete(advertisement.Id);

            Assert.DoesNotContain(advertisement, _repository.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenAdvertisementNotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(Guid.NewGuid()));
        }
    }
}
