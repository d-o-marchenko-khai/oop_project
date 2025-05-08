using System;

namespace oop_project
{
    public class SellingAdvertisement : Advertisement
    {
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
