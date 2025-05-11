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
            // Arrange
            var title = "Test Title";
            var description = "Test Description";
            var categoryId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            // Act
            var advertisement = new TestAdvertisement(title, description, categoryId, ownerId);

            // Assert
            Assert.Equal(title, advertisement.Title);
            Assert.Equal(description, advertisement.Description);
            Assert.Equal(categoryId, advertisement.CategoryId);
            Assert.Equal(ownerId, advertisement.OwnerId);
            Assert.True((DateTime.Now - advertisement.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Title_ShouldThrowValidationException_WhenTitleIsEmpty()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            Assert.Throws<ValidationException>(() => advertisement.Title = "");
        }

        [Fact]
        public void Description_ShouldThrowValidationException_WhenDescriptionIsEmpty()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            Assert.Throws<ValidationException>(() => advertisement.Description = "");
        }

        [Fact]
        public void Description_ShouldThrowValidationException_WhenDescriptionExceedsMaxLength()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());
            var longDescription = new string('a', 1001);

            // Act & Assert
            Assert.Throws<ValidationException>(() => advertisement.Description = longDescription);
        }

        [Fact]
        public void Publish_ShouldReturnTrue_WhenAdvertisementIsValid()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = advertisement.Publish();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Publish_ShouldReturnFalse_WhenCreatedAtIsInTheFuture()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddMinutes(1) // Set CreatedAt to the future
            };

            // Act
            var result = advertisement.Publish();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Unpublish_ShouldReturnTrue_WhenAdvertisementIsPublished()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());
            advertisement.Publish();

            // Act
            var result = advertisement.Unpublish();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Unpublish_ShouldReturnFalse_WhenAdvertisementIsNotPublished()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = advertisement.Unpublish();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CompareTo_ShouldReturnPositive_WhenOtherIsOlder()
        {
            // Arrange
            var advertisement1 = new TestAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now
            };
            var advertisement2 = new TestAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddMinutes(-1) // Older advertisement
            };

            // Act
            var result = advertisement1.CompareTo(advertisement2);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CompareTo_ShouldReturnNegative_WhenOtherIsNewer()
        {
            // Arrange
            var advertisement1 = new TestAdvertisement("Ad 1", "Description 1", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddMinutes(-1) // Older advertisement
            };
            var advertisement2 = new TestAdvertisement("Ad 2", "Description 2", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now
            };

            // Act
            var result = advertisement1.CompareTo(advertisement2);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Promote_ShouldReturnTrue_WhenPromotionIsAllowed()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddHours(-25) // Older than 24 hours
            };

            // Act
            var result = advertisement.Promote();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Promote_ShouldReturnFalse_WhenPromotionIsTooEarly()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Valid Title", "Valid Description", Guid.NewGuid(), Guid.NewGuid())
            {
                CreatedAt = DateTime.Now.AddHours(-1) // Less than 24 hours old
            };

            // Act
            var result = advertisement.Promote();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldUpdateProperties_WhenAdvertisementIsValid()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Old Title", "Old Description", Guid.NewGuid(), Guid.NewGuid());
            var updatedAdvertisement = new TestAdvertisement("New Title", "New Description", Guid.NewGuid(), Guid.NewGuid());

            // Act
            advertisement.Update(updatedAdvertisement);

            // Assert
            Assert.Equal("New Title", advertisement.Title);
            Assert.Equal("New Description", advertisement.Description);
            Assert.Equal(updatedAdvertisement.CategoryId, advertisement.CategoryId);
            Assert.Equal(updatedAdvertisement.OwnerId, advertisement.OwnerId);
        }

        [Fact]
        public void Update_ShouldThrowArgumentNullException_WhenAdvertisementIsNull()
        {
            // Arrange
            var advertisement = new TestAdvertisement("Old Title", "Old Description", Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => advertisement.Update(null));
        }
    }
}

