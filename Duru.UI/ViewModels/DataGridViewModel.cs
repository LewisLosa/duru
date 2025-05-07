using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Duru.UI.Contracts.ViewModels;
using Duru.UI.Core.Contracts.Services;
using Duru.UI.Core.Models;

namespace Duru.UI.ViewModels;

public partial class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRoomService _iRoomService;

    public DataGridViewModel(IRoomService iRoomService)
    {
        _iRoomService = iRoomService;
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
        iRoomService.CreateRoomAsync(testRoom).Wait();
    }


    public ObservableCollection<Room> Source
    {
        get;
    } = new();

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
}