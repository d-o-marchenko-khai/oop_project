using System;
using System.Collections.Generic;

namespace oop_project
{
    public class Guest : User
    {
        private readonly IChatRepository _chatRepository;
        private readonly IRegisteredUserRepository _registeredUserRepository;
        
        public Guest(IAdvertisementRepository advertisementRepository, IChatRepository chatRepository, IRegisteredUserRepository registeredUserRepository)
            : base(advertisementRepository)
        {
            _chatRepository = chatRepository;
            _registeredUserRepository = registeredUserRepository;
        }

        
        public RegisteredUser Register(RegisterUserDto dto)
        {
            
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
