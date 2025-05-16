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
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();

            CategoryRepository.Instance.Add(category);

            Assert.Contains(category, CategoryRepository.Instance.GetAll());
        }

        [Fact]
        public void Add_ShouldThrowArgumentNullException_WhenCategoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => CategoryRepository.Instance.Add(null));
        }

        [Fact]
        public void Add_ShouldThrowInvalidOperationException_WhenCategoryNameAlreadyExists()
        {
            var category = new Category("Duplicate Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            Assert.Throws<InvalidOperationException>(() => CategoryRepository.Instance.Add(new Category("Duplicate Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object)));
        }

        [Fact]
        public void GetAll_ShouldReturnAllCategories()
        {
            var category1 = new Category("Category 1", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            var category2 = new Category("Category 2", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category1);
            CategoryRepository.Instance.Add(category2);

            var categories = CategoryRepository.Instance.GetAll();

            Assert.Equal(2, categories.Count);
            Assert.Contains(category1, categories);
            Assert.Contains(category2, categories);
        }

        [Fact]
        public void GetById_ShouldReturnCategory_WhenIdExists()
        {
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            var result = CategoryRepository.Instance.GetById(category.Id);

            Assert.Equal(category, result);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            var result = CategoryRepository.Instance.GetById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public void GetByName_ShouldReturnCategory_WhenNameExists()
        {
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            var result = CategoryRepository.Instance.GetByName("Test Category");

            Assert.Equal(category, result);
        }

        [Fact]
        public void GetByName_ShouldReturnNull_WhenNameDoesNotExist()
        {
            var result = CategoryRepository.Instance.GetByName("Nonexistent Category");

            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldUpdateCategory_WhenCategoryExists()
        {
            var category = new Category("Old Name", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);
            var updatedCategory = new Category("New Name", CategoryRepository.Instance, _mockAdvertisementRepository.Object) { Id = category.Id };

            CategoryRepository.Instance.Update(updatedCategory);

            var result = CategoryRepository.Instance.GetById(category.Id);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenCategoryDoesNotExist()
        {
            var category = new Category("Nonexistent Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);

            Assert.Throws<KeyNotFoundException>(() => CategoryRepository.Instance.Update(category));
        }

        [Fact]
        public void Delete_ShouldRemoveCategory_WhenIdExists()
        {
            var category = new Category("Test Category", CategoryRepository.Instance, _mockAdvertisementRepository.Object);
            CategoryRepository.Instance.GetAll().Clear();
            CategoryRepository.Instance.Add(category);

            CategoryRepository.Instance.Delete(category.Id);

            Assert.DoesNotContain(category, CategoryRepository.Instance.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenIdDoesNotExist()
        {
            Assert.Throws<KeyNotFoundException>(() => CategoryRepository.Instance.Delete(Guid.NewGuid()));
        }
    }
}
