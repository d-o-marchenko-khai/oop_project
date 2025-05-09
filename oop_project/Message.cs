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
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Message text cannot be empty or whitespace.", nameof(value));
                }
                _text = value;
            }
        }

        public DateTime SentAt { get; set; }

        // Constructor
        public Message(Guid chatId, Guid senderId, string text)
        {
            if (chatId == Guid.Empty)
            {
                throw new ArgumentException("Chat ID must be a valid GUID.", nameof(chatId));
            }

            if (senderId == Guid.Empty)
            {
                throw new ArgumentException("Sender ID must be a valid GUID.", nameof(senderId));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Message text cannot be empty or whitespace.", nameof(text));
            }

            ChatId = chatId;
            SenderId = senderId;
            Text = text; // Validation is applied in the property setter
            SentAt = DateTime.Now;
        }
    }
}
