using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Collections.Generic;

namespace oop_project
{
    public enum AdvertisementType
    {
        Selling,
        Buying,
        Exchange
    }

    public abstract class Advertisement : IComparable<Advertisement>, IPublishable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        private string _title;
        private string _description;
        private bool _isPublished;
        public List<string> PhotoPaths { get; set; } = new List<string>();

        public Advertisement(string title, string description, Guid categoryId, Guid ownerId)
        {
            Title = title; 
            Description = description;
            CategoryId = categoryId;
            OwnerId = ownerId;
            CreatedAt = DateTime.Now;
        }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Title cannot be empty.");
                }
                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Description cannot be empty.");
                }
                if (value.Length > 1000)
                {
                    throw new ValidationException("Description cannot exceed 1000 characters.");
                }
                _description = value;
            }
        }

        public Guid CategoryId { get; set; }
        public Guid OwnerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Publish()
        {
            if (CreatedAt > DateTime.Now)
            {
                return false; 
            }

            _isPublished = true;
            return true;
        }

        public bool Unpublish()
        {
            if (!_isPublished)
            {
                return false; 
            }

            _isPublished = false;
            return true;
        }

        public int CompareTo(Advertisement other)
        {
            if (other == null) return 1;
            return CreatedAt.CompareTo(other.CreatedAt);
        }

        public bool Promote()
        {
            if ((DateTime.Now - this.CreatedAt).TotalHours < 24)
            {
                return false; 
            }
            this.CreatedAt = DateTime.Now; 
            return true;
        }

        public Advertisement Update(Advertisement advertisement)
        {
            if (advertisement == null)
            {
                throw new ArgumentNullException(nameof(advertisement));
            }
            Title = advertisement.Title;
            Description = advertisement.Description;
            CategoryId = advertisement.CategoryId;
            OwnerId = advertisement.OwnerId;
            PhotoPaths = advertisement.PhotoPaths;
            return this;
        }

        public virtual string ToJson()
        {
            return "";
        }
    }
}
