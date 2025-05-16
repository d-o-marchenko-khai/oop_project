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
            var name = "Test Category";

            var category = new Category(name, _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);

            Assert.Equal(name, category.Name);
            Assert.NotEqual(Guid.Empty, category.Id);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Category("Test Category", null, null));
        }

        [Fact]
        public void Name_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            var category = new Category("Valid Name", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);

            Assert.Throws<ArgumentException>(() => category.Name = "");
        }

        [Fact]
        public void AddCategory_ShouldAddCategory_WhenArgumentsAreValid()
        {
            var name = "New Category";

            var category = Category.AddCategory(name, _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);

            _mockCategoryRepository.Verify(repo => repo.Add(It.IsAny<Category>()), Times.Once);
            Assert.Equal(name, category.Name);
        }

        [Fact]
        public void AddCategory_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => Category.AddCategory("", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object));
        }

        [Fact]
        public void AddCategory_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Category.AddCategory("Test Category", null, null));
        }

        [Fact]
        public void Remove_ShouldDeleteCategory_WhenNoAdvertisementsExist()
        {
            var category = new Category("Test Category", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);
            _mockAdvertisementRepository.Setup(repo => repo.GetAll()).Returns(new List<Advertisement>());

            var result = category.Remove();

            _mockCategoryRepository.Verify(repo => repo.Delete(category.Id), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void Remove_ShouldThrowInvalidOperationException_WhenAdvertisementsExist()
        {
            var category = new Category("Test Category", _mockCategoryRepository.Object, _mockAdvertisementRepository.Object);
            var advertisements = new List<Advertisement>
            {
                new TestAdvertisement("Ad Title", "Ad Description", category.Id, Guid.NewGuid())
            };
            _mockAdvertisementRepository.Setup(repo => repo.GetAll()).Returns(advertisements);

            Assert.Throws<InvalidOperationException>(() => category.Remove());
        }
    }
}

