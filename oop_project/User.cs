using System;
using System.ComponentModel.DataAnnotations;

namespace oop_project
{
    public abstract class User
    {
        protected readonly IAdvertisementRepository _advertisementRepository;

        // Constructor to inject AdvertisementRepository
        protected User(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository ?? throw new ArgumentNullException(nameof(advertisementRepository));
        }

        // Views advertisements based on a filter
        public List<Advertisement> ViewAdvertisements(AdvertisementFilterDto filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
            }

            return _advertisementRepository.FindByFilters(filter);
        }
    }
}
