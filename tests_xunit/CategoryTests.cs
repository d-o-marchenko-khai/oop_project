using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;

namespace oop_project.Tests
{
    public class CategoryTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IAdvertisementRepository> _mockAdvertisementRepository;

        public CategoryTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockAdvertisementRepository = new Mock<IAdvertisementRepository>();
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenArgumentsAreValid()
        {
            // Arrange
            var name = "Test Category";

            // Act
            var category = new Category(name, _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);

            // Assert
            Assert.Equal(name, category.Name);
            Assert.NotEqual(Guid.Empty, category.Id);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Category("Test Category", null, null));
        }

        [Fact]
        public void Name_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            // Arrange
            var category = new Category("Valid Name", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => category.Name = "");
        }

        [Fact]
        public void AddCategory_ShouldAddCategory_WhenArgumentsAreValid()
        {
            // Arrange
            var name = "New Category";

            // Act
            var category = Category.AddCategory(name, _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.Add(It.IsAny<Category>()), Times.Once);
            Assert.Equal(name, category.Name);
        }

        [Fact]
        public void AddCategory_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Category.AddCategory("", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object));
        }

        [Fact]
        public void AddCategory_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Category.AddCategory("Test Category", null, null));
        }

        [Fact]
        public void Remove_ShouldDeleteCategory_WhenNoAdvertisementsExist()
        {
            // Arrange
            var category = new Category("Test Category", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);
            _mockAdvertisementRepository.Setup(repo => repo.GetAll()).Returns(new List<Advertisement>());

            // Act
            var result = category.Remove();

            // Assert
            _mockCategoryRepository.Verify(repo => repo.Delete(category.Id), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void Remove_ShouldThrowInvalidOperationException_WhenAdvertisementsExist()
        {
            // Arrange
            var category = new Category("Test Category", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);
            var advertisements = new List<Advertisement>
            {
                new TestAdvertisement("Ad Title", "Ad Description", category.Id, Guid.NewGuid())
            };
            _mockAdvertisementRepository.Setup(repo => repo.GetAll()).Returns(advertisements);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => category.Remove());
        }
    }
}

