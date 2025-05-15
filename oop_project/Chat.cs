using System;
using System.Collections.Generic;
using System.Text.Json;

namespace oop_project
{
    public class Chat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AdvertisementId { get; set; }
        public Tuple<Guid, Guid> ParticipantIds { get; set; }
        public DateTime CreatedAt { get; set; }
        private List<Message> Messages { get; set; } = new List<Message>();

        // Constructor
        public Chat(Guid advertisementId, Tuple<Guid, Guid> participantIds)
        {
            if (participantIds == null || participantIds.Item1 == Guid.Empty || participantIds.Item2 == Guid.Empty)
            {
                throw new ArgumentException("Participant IDs must be valid GUIDs.", nameof(participantIds));
            }

            AdvertisementId = advertisementId;
            ParticipantIds = participantIds;
            CreatedAt = DateTime.Now;
        }

        // Adds a message to the chat
        public Message AddMessage(Guid senderId, string text)
        {
            if (senderId == Guid.Empty)
            {
                throw new ArgumentException("Sender ID must be a valid GUID.", nameof(senderId));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Message text cannot be empty or whitespace.", nameof(text));
            }

            if (!ParticipantIds.Item1.Equals(senderId) && !ParticipantIds.Item2.Equals(senderId))
            {
                throw new InvalidOperationException("Sender is not a participant in this chat.");
            }

            var message = new Message(this.Id, senderId, text);

            Messages.Add(message);
            return message;
        }

        // Retrieves the chat history
        public List<Message> GetHistory()
        {
            return new List<Message>(Messages); // Return a copy to prevent external modification
        }

        public string ToJson()
        {
            var chatDto = new ChatJsonDto
            {
                Id = this.Id,
                AdvertisementId = this.AdvertisementId,
                ParticipantId1 = this.ParticipantIds.Item1,
                ParticipantId2 = this.ParticipantIds.Item2,
                CreatedAt = this.CreatedAt,
                Messages = this.Messages
            };
            
            return JsonSerializer.Serialize(chatDto, new JsonSerializerOptions { WriteIndented = false });
        }

        public static Chat FromJson(string json)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            try
            {
                // Create a temporary data transfer object to hold the JSON data
                var chatDto = JsonSerializer.Deserialize<ChatJsonDto>(json, jsonOptions);
                
                if (chatDto == null)
                {
                    throw new InvalidOperationException("Failed to deserialize chat data");
                }
                
                // Create a tuple for participant IDs
                var participantIds = new Tuple<Guid, Guid>(chatDto.ParticipantId1, chatDto.ParticipantId2);
                
                // Create the chat object
                var chat = new Chat(chatDto.AdvertisementId, participantIds);
                
                // Set properties that aren't in constructor
                chat.Id = chatDto.Id;
                chat.CreatedAt = chatDto.CreatedAt;
                
                // Add messages
                if (chatDto.Messages != null)
                {
                    chat.Messages = chatDto.Messages;
                }
                
                return chat;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing chat: {ex.Message}");
                Console.WriteLine($"JSON: {json}");
                throw;
            }
        }
    }

    // DTO class specifically for JSON serialization/deserialization
    public class ChatJsonDto
    {
        public Guid Id { get; set; }
        public Guid AdvertisementId { get; set; }
        public Guid ParticipantId1 { get; set; }
        public Guid ParticipantId2 { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
