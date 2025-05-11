using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
