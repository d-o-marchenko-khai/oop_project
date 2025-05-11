using System;
using System.Collections.Generic;

namespace oop_project
{
    public class Guest : User
    {
        private readonly IChatRepository _chatRepository;
        private readonly IRegisteredUserRepository _registeredUserRepository;
        // Constructor
        public Guest(IAdvertisementRepository advertisementRepository, IChatRepository chatRepository, IRegisteredUserRepository registeredUserRepository)
            : base(advertisementRepository)
        {
            _chatRepository = chatRepository;
            _registeredUserRepository = registeredUserRepository;
        }

        // Registers a guest as a registered user
        public RegisteredUser Register(RegisterUserDto dto)
        {
            // Example logic: Create a RegisteredUser using the DTO
            var registeredUser = new RegisteredUser(
                _advertisementRepository,
                _chatRepository,
                _registeredUserRepository,
                dto
            );

            _registeredUserRepository.Add(registeredUser);
            return registeredUser;
        }
    }
}
