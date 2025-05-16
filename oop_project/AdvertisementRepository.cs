using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace oop_project
{
    public interface IAdvertisementRepository
    {
        void Add(Advertisement advertisement);
        List<Advertisement> GetAll();
        Advertisement GetById(Guid id);
        List<Advertisement> GetByUserId(Guid userId);
        List<Advertisement> FindByFilters(AdvertisementFilterDto filter);
        void Update(Advertisement advertisement);
        void Delete(Guid id);
        string SerializeAll();
        void DeserializeAll(string json);
    }

    public class AdvertisementRepository : IAdvertisementRepository
    {
        private static AdvertisementRepository _instance;
        private static readonly object _lock = new();
        private readonly List<Advertisement> _advertisements = new();

        private AdvertisementRepository() { }

        public static AdvertisementRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new AdvertisementRepository();
                }
            }
        }

        public void Add(Advertisement advertisement)
        {
            if (advertisement == null)
            {
                throw new ArgumentNullException(nameof(advertisement));
            }
            _advertisements.Add(advertisement);
        }

        public List<Advertisement> GetAll()
        {
            return _advertisements;
        }

        public Advertisement GetById(Guid id)
        {
            return _advertisements.FirstOrDefault(ad => ad.Id == id);
        }

        public List<Advertisement> GetByUserId(Guid userId)
        {
            return _advertisements.Where(ad => ad.OwnerId == userId).ToList();
        }

        public List<Advertisement> FindByFilters(AdvertisementFilterDto filter)
        {
            IEnumerable<Advertisement> query = _advertisements;

            if (filter.Type.HasValue)
            {
                string typeString = filter.Type.ToString();
                query = query.Where(ad => ad.GetType().Name.Contains(typeString));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(ad => ad.CategoryId == filter.CategoryId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(ad =>
                    (ad is SellingAdvertisement sellingAd && sellingAd.Price >= filter.MinPrice.Value) ||
                    (ad is BuyingAdvertisement buyingAd && buyingAd.Price >= filter.MinPrice.Value));
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(ad =>
                    (ad is SellingAdvertisement sellingAd && sellingAd.Price <= filter.MaxPrice.Value) ||
                    (ad is BuyingAdvertisement buyingAd && buyingAd.Price <= filter.MaxPrice.Value));
            }

            return query.ToList();
        }

        public void Update(Advertisement advertisement)
        {
            var existingAd = GetById(advertisement.Id);
            if (existingAd == null)
            {
                throw new KeyNotFoundException("Advertisement not found.");
            }

            existingAd.Title = advertisement.Title;
            existingAd.Description = advertisement.Description;
            existingAd.CategoryId = advertisement.CategoryId;
            existingAd.OwnerId = advertisement.OwnerId;
        }

        public void Delete(Guid id)
        {
            var advertisement = GetById(id);
            if (advertisement == null)
            {
                throw new KeyNotFoundException("Advertisement not found.");
            }
            _advertisements.Remove(advertisement);
        }

        public string SerializeAll()
        {
            var wrappers = _advertisements.Select<Advertisement, object>(ad => ad switch
            {
                SellingAdvertisement s => new { Type = "Selling", Ad = s },
                BuyingAdvertisement b => new { Type = "Buying", Ad = b },
                ExchangeAdvertisement e => new { Type = "Exchange", Ad = e },
                _ => null
            }).Where(w => w != null).ToList();
            return JsonSerializer.Serialize(wrappers, new JsonSerializerOptions { WriteIndented = true });
        }

        public void DeserializeAll(string json)
        {
            _advertisements.Clear();
            if (string.IsNullOrWhiteSpace(json)) return;
            using var doc = JsonDocument.Parse(json);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var type = element.GetProperty("Type").GetString();
                var adJson = element.GetProperty("Ad").GetRawText();
                Advertisement ad = type switch
                {
                    "Selling" => SellingAdvertisement.FromJson(adJson),
                    "Buying" => BuyingAdvertisement.FromJson(adJson),
                    "Exchange" => ExchangeAdvertisement.FromJson(adJson),
                    _ => null
                };
                if (ad != null)
                    _advertisements.Add(ad);
            }
        }
    }
}
