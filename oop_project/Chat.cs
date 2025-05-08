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

        public Message AddMessage(Guid senderId, string text)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetHistory()
        {
            throw new NotImplementedException();
        }
    }
}
