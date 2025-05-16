using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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

        private RegisteredUserRepository() { }

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

        public List<RegisteredUser> GetAll()
        {
            return _users;
        }

        public RegisteredUser GetById(Guid id)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }

        public RegisteredUser GetByUsername(string username)
        {
            return _users.FirstOrDefault(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(RegisteredUser user)
        {
            var existingUser = GetById(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Phone = user.Phone;
        }

        public void Delete(Guid id)
        {
            var user = GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            _users.Remove(user);
        }

        public string SerializeAll()
        {
            return JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        }

        public void DeserializeAll(string json)
        {
            _users.Clear();
            if (string.IsNullOrWhiteSpace(json)) return;
            using var doc = JsonDocument.Parse(json);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var user = RegisteredUser.FromJson(element.GetRawText());
                if (user != null)
                    _users.Add(user);
            }
        }
    }
}
