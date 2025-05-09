using System;
using System.Collections.Generic;

namespace oop_project
{
    public class Guest : User
    {
        // Constructor
        public Guest()
        {
        }

        // Registers a guest as a registered user
        public RegisteredUser Register(RegisterUserDto dto)
        {
            // Example logic: Create a RegisteredUser using the DTO
            var registeredUser = new RegisteredUser(
                dto.Username,
                dto.Password,
                dto.FirstName,
                dto.LastName,
                dto.Phone
            );

            RegisteredUserRepository.Add(registeredUser);
            return registeredUser;
        }
    }
}
