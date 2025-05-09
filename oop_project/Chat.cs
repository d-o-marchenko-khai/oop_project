using System;
using System.Collections.Generic;

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
    }
}
