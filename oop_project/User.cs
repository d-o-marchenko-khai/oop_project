using System;

namespace oop_project
{
    public class User : IAuthenticatable
    {
        private string _username;
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _phone;

        public string Username
        {
            get => _username;
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                throw new NotImplementedException();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                throw new NotImplementedException();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Authenticate(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
