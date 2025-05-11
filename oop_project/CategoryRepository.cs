using System;
using System.Collections.Generic;
using System.Linq;

namespace oop_project
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        List<Category> GetAll();
        Category GetById(Guid id);
        Category GetByName(string name);
        void Update(Category category);
        void Delete(Guid id);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private static CategoryRepository _instance;
        private static readonly object _lock = new();
        private readonly List<Category> _categories = new();

        // Private constructor to prevent instantiation
        private CategoryRepository() { }

        // Singleton instance accessor
        public static CategoryRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new CategoryRepository();
                }
            }
        }

        // Add a new category
        public void Add(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (_categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A category with the same name already exists.");
            }
            _categories.Add(category);
        }

        // Get all categories
        public List<Category> GetAll()
        {
            return _categories;
        }

        // Find a category by ID
        public Category GetById(Guid id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        // Find a category by name
        public Category GetByName(string name)
        {
            return _categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Update an existing category
        public void Update(Category category)
        {
            var existingCategory = GetById(category.Id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            // Update properties
            existingCategory.Name = category.Name;
        }

        // Delete a category by ID
        public void Delete(Guid id)
        {
            var category = GetById(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            _categories.Remove(category);
        }
    }
}
