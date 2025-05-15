using ui.Components;
using MudBlazor.Services;
using oop_project;
using System;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Load serialized advertisements
    AdvertisementRepository.Instance.DeserializeAll(FileWriter.ReadString("ads.json"));
    Console.WriteLine("Advertisements loaded successfully.");
    
    // Load serialized users
    RegisteredUserRepository.Instance.DeserializeAll(FileWriter.ReadString("users.json"));
    Console.WriteLine("Users loaded successfully.");
    
    // Load serialized chats
    ChatRepository.Instance.DeserializeAll(FileWriter.ReadString("chats.json"));
    Console.WriteLine("Chats loaded successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading data: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Register application stopping event to save data
app.Lifetime.ApplicationStopping.Register(() =>
{
    try
    {
        // Save all data
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
