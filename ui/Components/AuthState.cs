using oop_project;

public static class AuthState
{
    // public static User CurrentUser { get; set; } = new Guest(
    //     AdvertisementRepository.Instance,
    //     ChatRepository.Instance,
    //     RegisteredUserRepository.Instance
    // );
    public static User CurrentUser { get; set; } = RegisteredUserRepository.Instance.GetAll().First();
} 