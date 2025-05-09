using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace tests
{
    [TestClass]
    public class GuestTests
    {
        [TestMethod]
        public void G_VIEW_01_ViewAdvertisements_AllMatches_ReturnsAll()
        {
            // Arrange
            var guest = new Guest();
            var advertisements = new List<Advertisement>
            {
                new SellingAdvertisement { Price = 50, CategoryId = Guid.NewGuid() },
                new BuyingAdvertisement { Price = 100, CategoryId = Guid.NewGuid() },
                new ExchangeAdvertisement { CategoryId = Guid.NewGuid() }
            };

            // Act
            var result = guest.ViewAdvertisements(new { MinPrice = 0, MaxPrice = 9999 });

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void G_VIEW_02_ViewAdvertisements_PriceRange_ReturnsFiltered()
        {
            // Arrange
            var guest = new Guest();
            var advertisements = new List<SellingAdvertisement>
            {
                new SellingAdvertisement { Price = 50 },
                new SellingAdvertisement { Price = 100 },
                new SellingAdvertisement { Price = 200 }
            };

            // Act
            var result = guest.ViewAdvertisements(new { MinPrice = 60, MaxPrice = 150 });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType<SellingAdvertisement>(result.First());
            Assert.AreEqual(100, (result.First() as SellingAdvertisement).Price);
        }

        [TestMethod]
        public void G_VIEW_03_ViewAdvertisements_TypeFilter_ReturnsFiltered()
        {
            // Arrange
            var guest = new Guest();
            var advertisements = new List<Advertisement>
            {
                new BuyingAdvertisement { Price = 100, CategoryId = Guid.NewGuid() },
                new SellingAdvertisement { Price = 50, CategoryId = Guid.NewGuid() },
                new ExchangeAdvertisement { CategoryId = Guid.NewGuid() }
            };

            // Act
            var result = guest.ViewAdvertisements(new { Types = new[] { "Selling" } });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType<SellingAdvertisement>(result.First());
        }

        [TestMethod]
        public void G_VIEW_04_ViewAdvertisements_CategoryFilter_ReturnsFiltered()
        {
            // Arrange
            var guest = new Guest();
            var categoryB = Guid.NewGuid();
            var advertisements = new List<Advertisement>
            {
                new SellingAdvertisement { CategoryId = Guid.NewGuid() },
                new BuyingAdvertisement { CategoryId = categoryB },
                new ExchangeAdvertisement { CategoryId = Guid.NewGuid() }
            };

            // Act
            var result = guest.ViewAdvertisements(new { CategoryId = categoryB });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(categoryB, result.First().CategoryId);
        }

        [TestMethod]
        public void G_VIEW_05_ViewAdvertisements_ComplexFilter_ReturnsFiltered()
        {
            // Arrange
            var guest = new Guest();
            var categoryC = Guid.NewGuid();
            var advertisements = new List<Advertisement>
            {
                new ExchangeAdvertisement { CategoryId = categoryC },
                new SellingAdvertisement { Price = 200, CategoryId = categoryC },
                new BuyingAdvertisement { Price = 100, CategoryId = Guid.NewGuid() }
            };

            // Act
            var result = guest.ViewAdvertisements(new { MinPrice = 50, MaxPrice = 150, Types = new[] { "Selling" }, CategoryId = categoryC });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType<SellingAdvertisement>(result.First());
            Assert.AreEqual(100, (result.First() as SellingAdvertisement).Price);
            Assert.AreEqual(categoryC, result.First().CategoryId);
        }

        [TestMethod]
        public void G_VIEW_06_ViewAdvertisements_NoMatches_ReturnsEmpty()
        {
            // Arrange
            var guest = new Guest();
            var advertisements = new List<Advertisement>();

            // Act
            var result = guest.ViewAdvertisements(new { MinPrice = 1000, MaxPrice = 2000, Types = new[] { "Buying" }, CategoryId = Guid.NewGuid() });

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void G_VIEW_07_ViewAdvertisements_ExactMatch_ReturnsFiltered()
        {
            // Arrange
            var guest = new Guest();
            var categoryA = Guid.NewGuid();
            var advertisements = new List<Advertisement>
            {
                new SellingAdvertisement { Price = 0, CategoryId = categoryA }
            };

            // Act
            var result = guest.ViewAdvertisements(new { MinPrice = 0, MaxPrice = 0, Types = new[] { "Selling" }, CategoryId = categoryA });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, (result.First() as SellingAdvertisement).Price);
            Assert.AreEqual(categoryA, result.First().CategoryId);
        }

        [TestMethod]
        public void G_REG_01_Register_UniqueUsername_ReturnsRegisteredUser()
        {
            // Arrange
            var guest = new Guest();

            // Act
            var user = guest.Register(new { Username = "newuser", Password = "abc123" });

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("newuser", user.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void G_REG_02_Register_DuplicateUsername_ThrowsValidationException()
        {
            // Arrange
            var guest = new Guest();
            var existingUser = new RegisteredUser { Username = "dupuser" };

            // Act
            guest.Register(new { Username = "dupuser", Password = "pass123" });
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void G_REG_03_Register_InvalidPassword_ThrowsValidationException()
        {
            // Arrange
            var guest = new Guest();

            // Act
            guest.Register(new { Username = "u", Password = "short" });
        }
    }
}
