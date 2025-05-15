using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace oop_project
{
    public interface IChatRepository
    {
        void Add(Chat chat);
        List<Chat> GetAll();
        Chat GetById(Guid id);
        List<Chat> GetByParticipant(Guid participantId);
        void Update(Chat chat);
        void Delete(Guid id);
        Chat GetByParticipantsAndAdvetrtisementId(Guid participant1, Guid participant2, Guid advertisementId);
    }

    public class ChatRepository : IChatRepository
    {
        private static ChatRepository _instance;
        private static readonly object _lock = new();
        private readonly List<Chat> _chats = new();

        // Private constructor to prevent instantiation
        private ChatRepository() { }

        // Singleton instance accessor
        public static ChatRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new ChatRepository();
                }
            }
        }

        // Add a new chat
        public void Add(Chat chat)
        {
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }
            _chats.Add(chat);
        }

        // Get all chats
        public List<Chat> GetAll()
        {
            return _chats;
        }

        // Find a chat by ID
        public Chat GetById(Guid id)
        {
            return _chats.FirstOrDefault(chat => chat.Id == id);
        }

        // Get chats by participant
        public List<Chat> GetByParticipant(Guid participantId)
        {
            return _chats.Where(chat => chat.ParticipantIds.Item1 == participantId || chat.ParticipantIds.Item2 == participantId).ToList();
        }

        public Chat GetByParticipantsAndAdvetrtisementId(Guid participant1, Guid participant2, Guid advertisementId)
        {
            return _chats.FirstOrDefault(chat =>
                (chat.ParticipantIds.Item1 == participant1 && chat.ParticipantIds.Item2 == participant2 ||
                 chat.ParticipantIds.Item1 == participant2 && chat.ParticipantIds.Item2 == participant1) &&
                chat.AdvertisementId == advertisementId);
        }

        // Update an existing chat
        public void Update(Chat chat)
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
        public void Delete(Guid id)
        {
            var chat = GetById(id);
            if (chat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }
            _chats.Remove(chat);
        }

        public string SerializeAll()
        {
            var chatDtos = _chats.Select(chat => new ChatJsonDto
            {
                Id = chat.Id,
                AdvertisementId = chat.AdvertisementId,
                ParticipantId1 = chat.ParticipantIds.Item1,
                ParticipantId2 = chat.ParticipantIds.Item2,
                CreatedAt = chat.CreatedAt,
                Messages = chat.GetHistory() // GetHistory() provides the list of messages
            }).ToList();
            return JsonSerializer.Serialize(chatDtos, new JsonSerializerOptions { WriteIndented = true });
        }

        public void DeserializeAll(string json)
        {
            _chats.Clear();
            if (string.IsNullOrWhiteSpace(json)) return;
            
            try
            {
                using var doc = JsonDocument.Parse(json);
                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    var chat = Chat.FromJson(element.GetRawText());
                    if (chat != null)
                        _chats.Add(chat);
                }
                Console.WriteLine($"Successfully deserialized {_chats.Count} chats");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing chats: {ex.Message}");
                Console.WriteLine($"JSON: {json}");
                throw;
            }
        }
    }
}
