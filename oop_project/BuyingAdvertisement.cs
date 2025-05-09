using System;

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
    }
}
