using System;

namespace oop_project
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        private string _name;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdvertisementRepository _advertisementRepository;

        public Category(string name, ICategoryRepository categoryRepository, IAdvertisementRepository advertisementRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }
            if (advertisementRepository == null)
            {
                throw new ArgumentNullException(nameof(advertisementRepository));
            }

            Name = name; // Validation is applied in the property setter
            _categoryRepository = categoryRepository;
            _advertisementRepository = advertisementRepository;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Category name cannot be empty or whitespace.", nameof(value));
                }
                _name = value;
            }
        }

        public static Category AddCategory(string name, ICategoryRepository categoryRepository, IAdvertisementRepository advertisementRepository)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty or whitespace.", nameof(name));
            }

            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            Category c = new Category(name, categoryRepository, advertisementRepository);

            categoryRepository.Add(c);

            return c;
        }

        public bool Remove()
        {
            int count = _advertisementRepository.GetAll()
                .Where(ad => ad.CategoryId == Id)
                .ToList()
                .Count;
            if (count > 0)
            {
                throw new InvalidOperationException("Cannot delete category with existing advertisements.");
            }

            _categoryRepository.Delete(Id);
            return true;
        }
    }
}
