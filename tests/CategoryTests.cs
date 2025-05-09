using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tests
{
    [TestClass]
    public class CategoryTests
    {
        private class TestAdvertisement : Advertisement
        {
            // A concrete implementation for testing purposes
        }

        private static List<Category> categories;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            categories = new List<Category>
            {
                new Category { Name = "Electronics" }
            };
        }

        [TestMethod]
        public void CAT_ADD_01_AddCategory_UniqueName_ReturnsNewCategory()
        {
            // Act
            var newCategory = Category.AddCategory("Books");

            // Assert
            Assert.IsNotNull(newCategory);
            Assert.AreEqual("Books", newCategory.Name);
            Assert.IsTrue(categories.Contains(newCategory));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void CAT_ADD_02_AddCategory_DuplicateName_ThrowsValidationException()
        {
            // Act
            Category.AddCategory("Electronics");
        }

        [TestMethod]
        public void CAT_REM_01_RemoveCategory_NoAdvertisements_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Name = "UnusedCategory" };

            // Act
            var result = category.Remove();

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(categories.Contains(category));
        }

        [TestMethod]
        public void CAT_REM_02_RemoveCategory_WithAdvertisements_ReturnsFalse()
        {
            // Arrange
            var category = new Category { Name = "UsedCategory" };
            var advertisement = new TestAdvertisement { CategoryId = category.Id };

            // Act
            var result = category.Remove();

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(categories.Contains(category));
        }
    }
}
