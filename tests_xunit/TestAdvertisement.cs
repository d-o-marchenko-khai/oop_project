using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace oop_project.Tests
{
    // Concrete subclass of Advertisement for testing purposes
    public class TestAdvertisement : Advertisement
    {
        public TestAdvertisement(string title, string description, Guid categoryId, Guid ownerId)
            : base(title, description, categoryId, ownerId)
        {
        }
    }

    public class AdvertisementTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenArgumentsAreValid()
        {
            var title = "Test Title";
            var description = "Test Description";
            var categoryId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            var advertisement = new TestAdvertisement(title, description, categoryId, ownerId);

            Assert.Equal(title, advertisement.Title);
            Assert.Equal(description, advertisement.Description);
            Assert.Equal(categoryId, advertisement.CategoryId);
            Assert.Equal(ownerId, advertisement.OwnerId);
            Assert.True((DateTime.Now - advertisement.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Title_ShouldThrowValidationException_WhenTitleIsEmpty()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            Assert.Throws<ValidationException>(() => advertisement.Title = "");
        }

        [Fact]
        public void Description_ShouldThrowValidationException_WhenDescriptionIsEmpty()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            Assert.Throws<ValidationException>(() => advertisement.Description = "");
        }

        [Fact]
        public void Description_ShouldThrowValidationException_WhenDescriptionExceedsMaxLength()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());
            var longDescription = new string('a', 1001);

            Assert.Throws<ValidationException>(() => advertisement.Description = longDescription);
        }

        [Fact]
        public void Publish_ShouldReturnTrue_WhenAdvertisementIsValid()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            var result = advertisement.Publish();

            Assert.True(result);
        }

        [Fact]
        public void Publish_ShouldReturnFalse_WhenCreatedAtIsInTheFuture()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddMinutes(1)
            };

            var result = advertisement.Publish();

            Assert.False(result);
        }

        [Fact]
        public void Unpublish_ShouldReturnTrue_WhenAdvertisementIsPublished()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());
            advertisement.Publish();

            var result = advertisement.Unpublish();

            Assert.True(result);
        }

        [Fact]
        public void Unpublish_ShouldReturnFalse_WhenAdvertisementIsNotPublished()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            var result = advertisement.Unpublish();

            Assert.False(result);
        }

        [Fact]
        public void CompareTo_ShouldReturnPositive_WhenOtherIsOlder()
        {
            var advertisement1 = new TestAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now
            };
            var advertisement2 = new TestAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddMinutes(-1)
            };

            var result = advertisement1.CompareTo(advertisement2);

            Assert.True(result > 0);
        }

        [Fact]
        public void CompareTo_ShouldReturnNegative_WhenOtherIsNewer()
        {
            var advertisement1 = new TestAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddMinutes(-1)
            };
            var advertisement2 = new TestAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now
            };

            var result = advertisement1.CompareTo(advertisement2);

            Assert.True(result < 0);
        }

        [Fact]
        public void Promote_ShouldReturnTrue_WhenPromotionIsAllowed()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddHours(-25)
            };

            var result = advertisement.Promote();

            Assert.True(result);
        }

        [Fact]
        public void Promote_ShouldReturnFalse_WhenPromotionIsTooEarly()
        {
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddHours(-1)
            };

            var result = advertisement.Promote();

            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldUpdateProperties_WhenAdvertisementIsValid()
        {
            var advertisement = new TestAdvertisement("Old Title", "Old Description", Guid.NewGuid(), Guid.NewGuid());
            var updatedAdvertisement = new TestAdvertisement("New Title", "New Description", Guid.NewGuid(), Guid.NewGuid());

            advertisement.Update(updatedAdvertisement);

            Assert.Equal("New Title", advertisement.Title);
            Assert.Equal("New Description", advertisement.Description);
            Assert.Equal(updatedAdvertisement.CategoryId, advertisement.CategoryId);
            Assert.Equal(updatedAdvertisement.OwnerId, advertisement.OwnerId);
        }

        [Fact]
        public void Update_ShouldThrowArgumentNullException_WhenAdvertisementIsNull()
        {
            var advertisement = new TestAdvertisement("Old Title", "Old Description", Guid.NewGuid(), Guid.NewGuid());

            Assert.Throws<ArgumentNullException>(() => advertisement.Update(null));
        }
    }
}

