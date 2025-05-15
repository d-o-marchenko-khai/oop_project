using System;
using System.Collections.Generic;

namespace oop_project
{
    public class CreateAdvertisementDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public AdvertisementType Type { get; set; }
        public int Price { get; set; } // Price for Selling and Buying
        public List<string> PhotoPaths { get; set; } = new List<string>();
    }
}
