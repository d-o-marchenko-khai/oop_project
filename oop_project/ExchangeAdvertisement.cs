using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace oop_project
{
    public class ExchangeAdvertisement : Advertisement
    {
        public ExchangeAdvertisement(
            string title,
            string description,
            Guid categoryId,
            Guid ownerId
        ) : base(title, description, categoryId, ownerId)
        {
        }

        public string ToJson()
        {
            var wrapper = new ExchangeAdWrapper { Type = "Exchange", Ad = this };
            return JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = false });
        }

        public static ExchangeAdvertisement FromJson(string json)
        {
            return JsonSerializer.Deserialize<ExchangeAdvertisement>(json);
        }

        private class ExchangeAdWrapper
        {
            public string Type { get; set; }
            public ExchangeAdvertisement Ad { get; set; }
        }
    }
}
