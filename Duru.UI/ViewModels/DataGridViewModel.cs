using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Duru.UI.Contracts.ViewModels;
using Duru.UI.Core.Contracts.Services;
using Duru.UI.Core.Models;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Duru.UI.ViewModels;

public partial class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRoomService _iRoomService;

    public DataGridViewModel(IRoomService iRoomService)
    {
        _iRoomService = iRoomService;
    }

    public ObservableCollection<Room> Source { get; } = new();

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _iRoomService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    void TestCommand()
    {
        System.Diagnostics.Debug.WriteLine("Test Komutu Çalıştı!");
    }


    [RelayCommand]
    async Task AddFunction() // Veya void AddFunction() eğer asenkron değilse
    {
        Room testRoom = new Room()
        {
            Name = "Test Room",
            Description = "This is a test room.",
            Capacity = 1,
            IsAvailable = true,
            Type = RoomType.Single,
            Status = RoomStatus.Available,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        await _iRoomService.CreateRoomAsync(testRoom); // Eğer AddFunction asenkron ise await kullanın
        new ToastContentBuilder()
    .AddArgument("action", "viewConversation")
    .AddArgument("conversationId", 9813)
    .AddText("Veri eklendi.")
    .AddText("Test verisi veritabanına eklendi.")
    .Show(); // Toast'u göster
    }
}