using System;

namespace oop_project
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        private string _name;

        public Category(string name)
        {
            Name = name; // Validation is applied in the property setter
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

        public static Category AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty or whitespace.", nameof(name));
            }

            Category c = new Category(name);

            CategoryRepository.Add(c);

            return c;
        }

        public bool Remove()
        {
            int count = AdvertisementRepository.GetAll()
                .Where(ad => ad.CategoryId == Id)
                .ToList()
                .Count;
            if (count > 0)
            {
                throw new InvalidOperationException("Cannot delete category with existing advertisements.");
            }

            CategoryRepository.Delete(Id);
            return true;
        }
    }
}
