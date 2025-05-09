using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;
using System.ComponentModel.DataAnnotations;

namespace tests
{
    [TestClass]
    public class SellingAdvertisementTests
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void SELL_PRICE_01_SetPrice_NegativeValue_ThrowsValidationException()
        {
            // Arrange
            var ad = new SellingAdvertisement();

            // Act
            ad.Price = -1;
        }

        [TestMethod]
        public void SELL_PRICE_02_SetPrice_ZeroValue_SetsPrice()
        {
            // Arrange
            var ad = new SellingAdvertisement();

            // Act
            ad.Price = 0;

            // Assert
            Assert.AreEqual(0, ad.Price);
        }

        [TestMethod]
        public void SELL_PRICE_03_SetPrice_PositiveValue_SetsPrice()
        {
            // Arrange
            var ad = new SellingAdvertisement();

            // Act
            ad.Price = 123.45m;

            // Assert
            Assert.AreEqual(123.45m, ad.Price);
        }
    }

    [TestClass]
    public class BuyingAdvertisementTests
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void BUY_PRICE_01_SetPrice_NegativeValue_ThrowsValidationException()
        {
            // Arrange
            var ad = new BuyingAdvertisement();

            // Act
            ad.Price = -10;
        }

        [TestMethod]
        public void BUY_PRICE_02_SetPrice_ZeroValue_SetsPrice()
        {
            // Arrange
            var ad = new BuyingAdvertisement();

            // Act
            ad.Price = 0;

            // Assert
            Assert.AreEqual(0, ad.Price);
        }

        [TestMethod]
        public void BUY_PRICE_03_SetPrice_PositiveValue_SetsPrice()
        {
            // Arrange
            var ad = new BuyingAdvertisement();

            // Act
            ad.Price = 99.99m;

            // Assert
            Assert.AreEqual(99.99m, ad.Price);
        }
    }
}

