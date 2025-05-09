namespace oop_project
{
    public class AdvertisementFilterDto
    {
        public AdvertisementType? Type { get; set; }
        public Guid? CategoryId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
