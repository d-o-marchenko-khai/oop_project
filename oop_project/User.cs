using System;
using System.ComponentModel.DataAnnotations;

namespace oop_project
{
    public abstract class User
    {
        // Views advertisements based on a filter
        public List<Advertisement> ViewAdvertisements(AdvertisementFilterDto filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
            }

            return AdvertisementRepository.FindByFilters(filter);
        }
    }
}
