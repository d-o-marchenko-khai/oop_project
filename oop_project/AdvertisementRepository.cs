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
        void SaveToFile(string path);
        void LoadFromFile(string path);
        string SerializeAll();
        void DeserializeAll(string json);
    }

    public class AdvertisementRepository : IAdvertisementRepository
    {
        private static AdvertisementRepository _instance;
        private static readonly object _lock = new();
        private readonly List<Advertisement> _advertisements = new();

        // Private constructor to prevent instantiation
        private AdvertisementRepository() { }

        // Singleton instance accessor
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

        // Add a new advertisement
        public void Add(Advertisement advertisement)
        {
            if (advertisement == null)
            {
                throw new ArgumentNullException(nameof(advertisement));
            }
            _advertisements.Add(advertisement);
        }

        // Get all advertisements
        public List<Advertisement> GetAll()
        {
            return _advertisements;
        }

        // Find an advertisement by ID
        public Advertisement GetById(Guid id)
        {
            return _advertisements.FirstOrDefault(ad => ad.Id == id);
        }

        // Find advertisements by user ID
        public List<Advertisement> GetByUserId(Guid userId)
        {
            return _advertisements.Where(ad => ad.OwnerId == userId).ToList();
        }

        // Find advertisements with filters
        public List<Advertisement> FindByFilters(AdvertisementFilterDto filter)
        {
            return _advertisements.Where(ad =>
                (!filter.Type.HasValue || ad.GetType().Name.Contains(filter.Type.ToString())) &&
                (!filter.CategoryId.HasValue || ad.CategoryId == filter.CategoryId) &&
                (!filter.MinPrice.HasValue ||
                    ((ad is SellingAdvertisement sellingAd && sellingAd.Price >= filter.MinPrice) ||
                     (ad is BuyingAdvertisement buyingAd && buyingAd.Price >= filter.MinPrice))) &&
                (!filter.MaxPrice.HasValue ||
                    ((ad is SellingAdvertisement sellingAd2 && sellingAd2.Price <= filter.MaxPrice) ||
                     (ad is BuyingAdvertisement buyingAd2 && buyingAd2.Price <= filter.MaxPrice)))
            ).ToList();
        }

        // Update an existing advertisement
        public void Update(Advertisement advertisement)
        {
            var existingAd = GetById(advertisement.Id);
            if (existingAd == null)
            {
                throw new KeyNotFoundException("Advertisement not found.");
            }

            // Update properties
            existingAd.Title = advertisement.Title;
            existingAd.Description = advertisement.Description;
            existingAd.CategoryId = advertisement.CategoryId;
            existingAd.OwnerId = advertisement.OwnerId;
        }

        // Delete an advertisement by ID
        public void Delete(Guid id)
        {
            var advertisement = GetById(id);
            if (advertisement == null)
            {
                throw new KeyNotFoundException("Advertisement not found.");
            }
            _advertisements.Remove(advertisement);
        }

        public void SaveToFile(string path)
        {
            using var writer = new StreamWriter(path);
            foreach (var ad in _advertisements)
            {
                string json = ad switch
                {
                    SellingAdvertisement s => s.ToJson(),
                    BuyingAdvertisement b => b.ToJson(),
                    ExchangeAdvertisement e => e.ToJson(),
                    _ => null
                };
                if (json != null)
                    writer.WriteLine(json);
            }
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;
            _advertisements.Clear();
            foreach (var line in File.ReadLines(path))
            {
                using var doc = JsonDocument.Parse(line);
                var type = doc.RootElement.GetProperty("Type").GetString();
                Advertisement ad = type switch
                {
                    "Selling" => SellingAdvertisement.FromJson(line),
                    "Buying" => BuyingAdvertisement.FromJson(line),
                    "Exchange" => ExchangeAdvertisement.FromJson(line),
                    _ => null
                };
                if (ad != null)
                    _advertisements.Add(ad);
            }
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
