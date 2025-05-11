using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace oop_project.Tests
{
    public class CategoryRepositoryTests
    {
        private readonly Mock<IAdvertisementRepository> _mockAdvertisementRepository;

        public CategoryRepositoryTests()
        {
            _mockAdvertisementRepository = new Mock<IAdvertisementRepository>();
        }

        [Fact]
        public void Add_ShouldAddCategory_WhenCategoryIsValid()
        {
            // Arrange
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear(); // Ensure a clean state

            // Act
            CategoryRepository.Instance.Add(category);

            // Assert
            Assert.Contains(category, CategoryRepository.Instance.GetAll());
        }

        [Fact]
        public void Add_ShouldThrowArgumentNullException_WhenCategoryIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CategoryRepository.Instance.Add(null));
        }

        [Fact]
        public void Add_ShouldThrowInvalidOperationException_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            var category = new Category("Duplicate Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => CategoryRepository.Instance.Add(new Category("Duplicate Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object)));
        }

        [Fact]
        public void GetAll_ShouldReturnAllCategories()
        {
            // Arrange
            var category1 = new Category("Category 1", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            var category2 = new Category("Category 2", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category1);
            CategoryRepository.Instance.Add(category2);

            // Act
            var categories = CategoryRepository.Instance.GetAll();

            // Assert
            Assert.Equal(2, categories.Count);
            Assert.Contains(category1, categories);
            Assert.Contains(category2, categories);
        }

        [Fact]
        public void GetById_ShouldReturnCategory_WhenIdExists()
        {
            // Arrange
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            // Act
            var result = CategoryRepository.Instance.GetById(category.Id);

            // Assert
            Assert.Equal(category, result);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Act
            var result = CategoryRepository.Instance.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetByName_ShouldReturnCategory_WhenNameExists()
        {
            // Arrange
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            // Act
            var result = CategoryRepository.Instance.GetByName("Test Category");

            // Assert
            Assert.Equal(category, result);
        }

        [Fact]
        public void GetByName_ShouldReturnNull_WhenNameDoesNotExist()
        {
            // Act
            var result = CategoryRepository.Instance.GetByName("Nonexistent Category");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldUpdateCategory_WhenCategoryExists()
        {
            // Arrange
            var category = new Category("Old Name", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);
            var updatedCategory = new Category("New Name", CategoryRepository.Instance, _mockAdvertisementRepository.Object) { Id = category.Id };

            // Act
            CategoryRepository.Instance.Update(updatedCategory);

            // Assert
            var result = CategoryRepository.Instance.GetById(category.Id);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var category = new Category("Nonexistent Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => CategoryRepository.Instance.Update(category));
        }

        [Fact]
        public void Delete_ShouldRemoveCategory_WhenIdExists()
        {
            // Arrange
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            // Act
            CategoryRepository.Instance.Delete(category.Id);

            // Assert
            Assert.DoesNotContain(category, CategoryRepository.Instance.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenIdDoesNotExist()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => CategoryRepository.Instance.Delete(Guid.NewGuid()));
        }
    }
}
