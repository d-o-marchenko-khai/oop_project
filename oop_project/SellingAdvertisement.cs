using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace oop_project
{
    public class SellingAdvertisement : Advertisement
    {
        private decimal _price;

        public SellingAdvertisement(string title, string description, Guid categoryId, Guid ownerId, decimal price)
            : base(title, description, categoryId, ownerId)
        {
            Price = price; 
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
            var wrapper = new SellingAdWrapper { Type = "Selling", Ad = this };
            return JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = false });
        }

        public static SellingAdvertisement FromJson(string json)
        {
            return JsonSerializer.Deserialize<SellingAdvertisement>(json);
        }

        private class SellingAdWrapper
        {
            public string Type { get; set; }
            public SellingAdvertisement Ad { get; set; }
        }
    }
}
