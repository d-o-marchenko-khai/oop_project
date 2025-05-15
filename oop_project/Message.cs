using System;
using System.Text.Json;

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

        // Default constructor for JSON deserialization
        public Message() { }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = false });
        }

        public static Message FromJson(string json)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            try
            {
                var message = JsonSerializer.Deserialize<Message>(json, jsonOptions);
                
                if (message == null)
                {
                    throw new InvalidOperationException("Failed to deserialize message data");
                }
                
                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing message: {ex.Message}");
                Console.WriteLine($"JSON: {json}");
                throw;
            }
        }
    }
}
