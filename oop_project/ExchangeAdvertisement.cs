using System;

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
    }
}
