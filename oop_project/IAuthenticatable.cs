namespace oop_project
{
    public interface IAuthenticatable
    {
        bool Authenticate(string login, string password);
    }
}
