using System;
using System.Collections.Generic;
using System.Linq;

namespace oop_project
{
    public static class ChatRepository
    {
        private readonly static List<Chat> _chats = new();

        // Add a new chat
        public static void Add(Chat chat)
        {
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }
            _chats.Add(chat);
        }

        // Get all chats
        public static List<Chat> GetAll()
        {
            return _chats;
        }

        // Find a chat by ID
        public static Chat GetById(Guid id)
        {
            return _chats.FirstOrDefault(chat => chat.Id == id);
        }

        // Get chats by participant
        public static List<Chat> GetByParticipant(Guid participantId)
        {
            return (List <Chat>)_chats.Where(chat => chat.ParticipantIds.Item1 == participantId || chat.ParticipantIds.Item2 == participantId);
        }

        // Update an existing chat
        public static void Update(Chat chat)
        {
            var existingChat = GetById(chat.Id);
            if (existingChat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }

            // Update properties
            existingChat.ParticipantIds = chat.ParticipantIds;
            existingChat.AdvertisementId = chat.AdvertisementId;
        }

        // Delete a chat by ID
        public static void Delete(Guid id)
        {
            var chat = GetById(id);
            if (chat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }
            _chats.Remove(chat);
        }
    }
}
