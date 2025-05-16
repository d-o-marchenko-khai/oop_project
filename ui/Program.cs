using ui.Components;
using MudBlazor.Services;
using oop_project;
using System;

var builder = WebApplication.CreateBuilder(args);

try
{
    AdvertisementRepository.Instance.DeserializeAll(FileWriter.ReadString("ads.json"));
    Console.WriteLine("Advertisements loaded successfully.");
    
    RegisteredUserRepository.Instance.DeserializeAll(FileWriter.ReadString("users.json"));
    Console.WriteLine("Users loaded successfully.");
    
    ChatRepository.Instance.DeserializeAll(FileWriter.ReadString("chats.json"));
    Console.WriteLine("Chats loaded successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading data: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Lifetime.ApplicationStopping.Register(() =>
{
    try
    {
        FileWriter.WriteString("ads.json", AdvertisementRepository.Instance.SerializeAll());
        FileWriter.WriteString("users.json", RegisteredUserRepository.Instance.SerializeAll());
        FileWriter.WriteString("chats.json", ChatRepository.Instance.SerializeAll());
        Console.WriteLine("All data saved successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving data: {ex.Message}");
    }
});

app.Run();
