using System;

namespace oop_project
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        
        private string _text;
        public string Text 
        {
            get => _text;
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime SentAt { get; set; }
    }
}
