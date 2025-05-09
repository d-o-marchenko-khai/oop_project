using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop_project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace tests
{
    [TestClass]
    public class RegisteredUserTests
    {
        [TestMethod]
        public void RU_CREATE_01_CreateAdvertisement_ValidInput_AddsAdvertisement()
        {
            // Arrange
            var user = new RegisteredUser();
            var dto = new { Title = "A", Description = "B", CategoryId = Guid.NewGuid(), Type = "Selling" };

            // Act
            var ad = user.CreateAdvertisement(dto);

            // Assert
            Assert.IsNotNull(ad);
            Assert.AreEqual("A", ad.Title);
            Assert.AreEqual("B", ad.Description);
            Assert.AreEqual(dto.CategoryId, ad.CategoryId);
            Assert.IsTrue(user.Advertisements.Contains(ad));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RU_CREATE_02_CreateAdvertisement_EmptyTitle_ThrowsValidationException()
        {
            // Arrange
            var user = new RegisteredUser();
            var dto = new { Title = "", Description = "B", CategoryId = Guid.NewGuid(), Type = "Selling" };

            // Act
            user.CreateAdvertisement(dto);
        }

        [TestMethod]
        public void RU_SEND_01_SendMessage_ValidChat_AddsMessage()
        {
            // Arrange
            var user1 = new RegisteredUser();
            var user2 = new RegisteredUser();
            var chat = new Chat(Guid.NewGuid(), new Tuple<Guid, Guid>(user1.Id, user2.Id));
            user1.Chats.Add(chat);

            // Act
            var message = user1.SendMessage(chat.Id, "Hello");

            // Assert
            Assert.IsNotNull(message);
            Assert.AreEqual("Hello", message.Text);
            Assert.AreEqual(user1.Id, message.SenderId);
            Assert.IsTrue((DateTime.Now - message.SentAt).TotalSeconds < 1);
            Assert.IsTrue(chat.GetHistory().Contains(message));
        }

        [TestMethod]
        public void RU_MANAGE_01_ManageAdvertisement_OlderThan24Hours_PromotesSuccessfully()
        {
            // Arrange
            var user = new RegisteredUser();
            var ad = new SellingAdvertisement
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now.AddDays(-1),
                Price = 100,
            };
            user.Advertisements.Add(ad);

            // Act
            var result = user.ManageAdvertisement(ad.Id, "Promote");

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue((DateTime.Now - ad.CreatedAt).TotalSeconds < 1);
        }

        [TestMethod]
        public void RU_MANAGE_02_ManageAdvertisement_YoungerThan24Hours_FailsToPromote()
        {
            // Arrange
            var user = new RegisteredUser();
            var ad = new SellingAdvertisement
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now.AddHours(-12),
                Price = 100,
            };
            user.Advertisements.Add(ad);

            // Act
            var result = user.ManageAdvertisement(ad.Id, "Promote");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
