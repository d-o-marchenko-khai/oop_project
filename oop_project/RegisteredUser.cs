using System;
using System.Collections.Generic;

namespace oop_project
{
    public class RegisteredUser : User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
        public List<Chat> Chats { get; set; } = new List<Chat>();

        public Advertisement CreateAdvertisement(object dto)
        {
            throw new NotImplementedException();
        }

        public Message SendMessage(Guid chatId, string text)
        {
            throw new NotImplementedException();
        }

        public bool ManageAdvertisement(Guid adId, string action)
        {
            throw new NotImplementedException();
        }
    }
}
