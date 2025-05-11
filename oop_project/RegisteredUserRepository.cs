using System;
using System.Collections.Generic;
using System.Linq;

namespace oop_project
{
    public interface IRegisteredUserRepository
    {
        void Add(RegisteredUser user);
        List<RegisteredUser> GetAll();
        RegisteredUser GetById(Guid id);
        RegisteredUser GetByUsername(string username);
        void Update(RegisteredUser user);
        void Delete(Guid id);
    }

    public class RegisteredUserRepository : IRegisteredUserRepository
    {
        private static RegisteredUserRepository _instance;
        private static readonly object _lock = new();
        private readonly List<RegisteredUser> _users = new();

        // Private constructor to prevent instantiation
        private RegisteredUserRepository() { }

        // Singleton instance accessor
        public static RegisteredUserRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new RegisteredUserRepository();
                }
            }
        }

        // Add a new user
        public void Add(RegisteredUser user)
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
        public List<RegisteredUser> GetAll()
        {
            return _users;
        }

        // Find a user by ID
        public RegisteredUser GetById(Guid id)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }

        // Find a user by username
        public RegisteredUser GetByUsername(string username)
        {
            return _users.FirstOrDefault(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Update an existing user
        public void Update(RegisteredUser user)
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
        public void Delete(Guid id)
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
