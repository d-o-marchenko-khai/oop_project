using System;
using System.Collections.Generic;
using System.Linq;

namespace oop_project
{
    public static class RegisteredUserRepository
    {
        private readonly static List<RegisteredUser> _users = new();

        // Add a new user
        public static void Add(RegisteredUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (_users.Any(u => u.Username == user.Username))
            {
                throw new InvalidOperationException("A user with the same username already exists.");
            }
            _users.Add(user);
        }

        // Get all users
        public static List<RegisteredUser> GetAll()
        {
            return _users;
        }

        // Find a user by ID
        public static RegisteredUser GetById(Guid id)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }

        // Find a user by username
        public static RegisteredUser GetByUsername(string username)
        {
            return _users.First(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Update an existing user
        public static void Update(RegisteredUser user)
        {
            var existingUser = GetById(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Update properties
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Phone = user.Phone;
        }

        // Delete a user by ID
        public static void Delete(Guid id)
        {
            var user = GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            _users.Remove(user);
        }
    }
}
