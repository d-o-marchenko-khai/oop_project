using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace oop_project
{
    public class RegisteredUser : User, IAuthenticatable
    {
        private string _username;
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _phone;
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<Advertisement> Advertisements {
            get
            {
                return AdvertisementRepository.GetByUserId(Id);
            }
            set {}
        }
        public List<Chat> Chats {
            get
            {
                return ChatRepository.GetByParticipant(Id);
            }
            set {}
        }

        public RegisteredUser(string username, string password, string firstName, string lastName, string phone)
        {
            Username = username; // Validation is applied in the property setter
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                {
                    throw new ValidationException("Username must be at least 6 characters long and cannot be empty.");
                }
                if (!IsUsernameUnique(value))
                {
                    throw new ValidationException("Username must be unique.");
                }
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6 || !HasDigit(value))
                {
                    throw new ValidationException("Password must be at least 6 characters long, cannot be empty, and must contain at least one digit.");
                }
                _password = value;
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 1)
                {
                    throw new ValidationException("First name cannot be empty.");
                }
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 1)
                {
                    throw new ValidationException("Last name cannot be empty.");
                }
                _lastName = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Phone cannot be empty.");
                }
                _phone = value;
            }
        }

        public bool Authenticate(string login, string password)
        {
            return _username == login && _password == password;
        }

        private bool HasDigit(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsUsernameUnique(string username)
        {
            return RegisteredUserRepository.GetByUsername(username) == null;
        }

        public Advertisement CreateAdvertisement(CreateAdvertisementDto dto)
        {
            Advertisement advertisement;

            // Determine the type of advertisement
            switch (dto.Type)
            {
                case AdvertisementType.Selling:
                    advertisement = new SellingAdvertisement(dto.Title, dto.Description, dto.CategoryId, this.Id, dto.Price);
                    break;

                case AdvertisementType.Buying:
                    advertisement = new BuyingAdvertisement(dto.Title, dto.Description, dto.CategoryId, this.Id, dto.Price);
                    break;

                case AdvertisementType.Exchange:
                    advertisement = new ExchangeAdvertisement(dto.Title, dto.Description, dto.CategoryId, this.Id);
                    break;

                default:
                    throw new ValidationException("Invalid advertisement type.");
            }

            // Add to user's advertisements
            AdvertisementRepository.Add(advertisement);

            return advertisement;
        }

        public Message SendMessage(Guid chatId, string text)
        {
            // Find the chat
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat == null)
            {
                throw new InvalidOperationException("Chat not found or user is not a participant.");
            }

            // Add message
            var message = chat.AddMessage(this.Id, text);
            return message;
        }

        public List<Message> GetChatHistory(Guid chatId)
        {
            // Find the chat
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat == null)
            {
                throw new InvalidOperationException("Chat not found or user is not a participant.");
            }
            // Retrieve chat history
            return chat.GetHistory();
        }

        public Chat ContactAdvertisementOwner(Guid advertisementId)
        {
            // Find the advertisement
            var advertisement = AdvertisementRepository.GetById(advertisementId);
            if (advertisement == null)
            {
                throw new InvalidOperationException("Advertisement not found.");
            }
            // Create a new chat with the advertisement owner
            var chat = new Chat(advertisementId, new Tuple<Guid, Guid>(this.Id, advertisement.OwnerId));
            ChatRepository.Add(chat);
            return chat;
        }

        public bool PromoteAdvertisement(Guid adId)
        {
            var advertisement = Advertisements.FirstOrDefault(ad => ad.Id == adId);
            if (advertisement == null)
            {
                throw new InvalidOperationException("Advertisement not found.");
            }

            advertisement.Promote();
            return true;
        }

        public bool UpdateAdvertisement(Guid adId, Advertisement ad)
        {
            var advertisement = Advertisements.FirstOrDefault(ad => ad.Id == adId);
            if (advertisement == null)
            {
                throw new InvalidOperationException("Advertisement not found.");
            }

            advertisement.Update(ad);
            return true;
        }

        public bool DeleteAdvertisement(Guid adId)
        {
            var advertisement = Advertisements.FirstOrDefault(ad => ad.Id == adId);
            if (advertisement == null)
            {
                throw new InvalidOperationException("Advertisement not found.");
            }

            AdvertisementRepository.Delete(advertisement.Id);
            return true;
        }
    }
}
