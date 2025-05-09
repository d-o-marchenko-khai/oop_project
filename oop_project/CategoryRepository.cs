using System;
using System.Collections.Generic;
using System.Linq;

namespace oop_project
{
    public static class CategoryRepository
    {
        private static readonly List<Category> _categories = new();

        // Add a new category
        public static void Add(Category category)
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
        public static List<Category> GetAll()
        {
            return _categories;
        }

        // Find a category by ID
        public static Category GetById(Guid id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        // Find a category by name
        public static Category GetByName(string name)
        {
            return _categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Update an existing category
        public static void Update(Category category)
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
        public static void Delete(Guid id)
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
