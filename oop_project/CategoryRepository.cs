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

        private CategoryRepository() { }

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

        public List<Category> GetAll()
        {
            return _categories;
        }

        public Category GetById(Guid id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public Category GetByName(string name)
        {
            return _categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(Category category)
        {
            var existingCategory = GetById(category.Id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            existingCategory.Name = category.Name;
        }

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
