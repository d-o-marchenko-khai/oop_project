using System;

namespace oop_project
{

    public abstract class Advertisement : IComparable<Advertisement>, IPublishable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        private string _title;
        private string _description;

        public string Title
        {
            get => _title;
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool Publish()
        {
            throw new NotImplementedException();
        }

        public bool Unpublish()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Advertisement other)
        {
            throw new NotImplementedException();
        }
    }
}
