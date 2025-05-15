using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace oop_project
{
    public class BuyingAdvertisement : Advertisement
    {
        private decimal _price;

        public BuyingAdvertisement(string title, string description, Guid categoryId, Guid ownerId, decimal price)
            : base(title, description, categoryId, ownerId)
        {
            Price = price; // Validation is applied in the property setter
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be negative.");
                }
                _price = value;
            }
        }

        public override string ToJson()
        {
            var wrapper = new BuyingAdWrapper { Type = "Buying", Ad = this };
            return JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = false });
        }

        public static BuyingAdvertisement FromJson(string json)
        {
            return JsonSerializer.Deserialize<BuyingAdvertisement>(json);
        }

        private class BuyingAdWrapper
        {
            public string Type { get; set; }
            public BuyingAdvertisement Ad { get; set; }
        }
    }
}
