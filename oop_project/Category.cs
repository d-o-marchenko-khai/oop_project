using System;

namespace oop_project
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        private string _name;

        public string Name 
        {
            get => _name;
            set
            {
                throw new NotImplementedException();
            }
        }

        public static Category AddCategory(string name)
        {
            throw new NotImplementedException();
        }

        public bool Remove()
        {
            throw new NotImplementedException();
        }
    }
}
