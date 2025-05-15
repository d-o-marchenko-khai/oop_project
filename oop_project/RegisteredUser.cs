using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private readonly IChatRepository _chatRepository;
        private readonly IRegisteredUserRepository _registeredUserRepository;

        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public List<Advertisement> Advertisements
        {
            get
            {
                return _advertisementRepository.GetByUserId(Id) ?? new List<Advertisement>();
            }
            set { }
        }

        [JsonIgnore]
        public List<Chat> Chats
        {
            get
            {
                return _chatRepository.GetByParticipant(Id) ?? new List<Chat>();
            }
            set { }
        }

        // Constructor to inject AdvertisementRepository and ChatRepository
        public RegisteredUser(
            IAdvertisementRepository advertisementRepository,
            IChatRepository chatRepository,
            IRegisteredUserRepository registeredUserRepository,
            RegisterUserDto dto
        ) : base(advertisementRepository)
        {
            _chatRepository = chatRepository;
            _registeredUserRepository = registeredUserRepository;
            Username = dto.Username; // Validation is applied in the property setter
            Password = dto.Password;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Phone = dto.Phone;
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
            return _registeredUserRepository.GetByUsername(username) == null;
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
            _advertisementRepository.Add(advertisement);

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
            var advertisement = _advertisementRepository.GetById(advertisementId);
            if (advertisement == null)
            {
                throw new InvalidOperationException("Advertisement not found.");
            }
            if (advertisement.OwnerId == this.Id)
            {
                throw new InvalidOperationException("Cannot contact yourself.");
            }
            // Check if a chat already exists
            var existingChat = _chatRepository.GetByParticipantsAndAdvetrtisementId(this.Id, advertisement.OwnerId, advertisementId);
            if (existingChat != null)
            {
                return existingChat;
            }
            // Create a new chat with the advertisement owner
            var chat = new Chat(advertisementId, new Tuple<Guid, Guid>(this.Id, advertisement.OwnerId));
            _chatRepository.Add(chat);
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

            _advertisementRepository.Delete(advertisement.Id);
            return true;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = false });
        }

        public static RegisteredUser FromJson(string json)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            try 
            {
                // Create a temporary data transfer object to hold the JSON data
                var userDto = JsonSerializer.Deserialize<RegisteredUserJsonDto>(json, jsonOptions);
                
                if (userDto == null)
                {
                    throw new InvalidOperationException("Failed to deserialize user data");
                }
                
                // Get the repository instances
                var adRepository = AdvertisementRepository.Instance;
                var chatRepository = ChatRepository.Instance;
                var userRepository = RegisteredUserRepository.Instance;
                
                // Create the proper RegisteredUser object with dependencies
                var registerDto = new RegisterUserDto
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Phone = userDto.Phone
                };
                
                var user = new RegisteredUser(adRepository, chatRepository, userRepository, registerDto);
                
                // Set the Id from the deserialized value - ensure it's parsed correctly
                user.Id = userDto.Id;
                
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing user: {ex.Message}");
                Console.WriteLine($"JSON: {json}");
                throw;
            }
        }
    }
    
    // DTO class specifically for JSON deserialization
    public class RegisteredUserJsonDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
