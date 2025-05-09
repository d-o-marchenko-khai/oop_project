using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;

namespace tests
{
    [TestClass]
    public class AdvertisementTests
    {
        private class TestAdvertisement : Advertisement
        {
            // A concrete implementation for testing purposes
        }

        [TestMethod]
        public void ADV_PUB_01_Publish_CreatedAtInPast_ReturnsTrue()
        {
            // Arrange
            var ad = new TestAdvertisement { CreatedAt = DateTime.Now.AddHours(-1) };

            // Act
            var result = ad.Publish();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ADV_PUB_02_Publish_CreatedAtInFuture_ReturnsFalse()
        {
            // Arrange
            var ad = new TestAdvertisement { CreatedAt = DateTime.Now.AddHours(1) };

            // Act
            var result = ad.Publish();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ADV_UNPUB_01_Unpublish_PublishedAdvertisement_ReturnsTrue()
        {
            // Arrange
            var ad = new TestAdvertisement();
            ad.Publish(); // Assuming Publish sets some internal state

            // Act
            var result = ad.Unpublish();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ADV_PROM_01_Promote_LastPromotionMoreThan24HoursAgo_ReturnsTrueAndUpdatesCreatedAt()
        {
            // Arrange
            var ad = new TestAdvertisement { CreatedAt = DateTime.Now.AddDays(-1) };

            // Act
            var result = ad.Promote();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue((DateTime.Now - ad.CreatedAt).TotalSeconds < 1);
        }

        [TestMethod]
        public void ADV_PROM_02_Promote_LastPromotionLessThan24HoursAgo_ReturnsFalse()
        {
            // Arrange
            var ad = new TestAdvertisement { CreatedAt = DateTime.Now.AddHours(-12) };

            // Act
            var result = ad.Promote();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
