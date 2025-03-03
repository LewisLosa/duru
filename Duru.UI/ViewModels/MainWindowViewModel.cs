using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Duru.Application.DTOs.Room;
using Duru.Application.Interfaces;
using Duru.Domain.Enums;

namespace Duru.UI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IRoomService _roomService;

        // CommunityToolkit’in [ObservableProperty] özelliği sayesinde
        // otomatik PropertyChanged bildirimleri yapılır.
        [ObservableProperty]
        private ObservableCollection<RoomDto> _rooms = new();

        public MainWindowViewModel(IRoomService roomService)
        {
            _roomService = roomService;
            LoadRoomsCommand = new RelayCommand(async () => await LoadRoomsAsync());
            AddRoomCommand = new RelayCommand(async () => await AddNewRoomAsync());

            // Uygulama başladığında odaları yükle.
            _ = LoadRoomsAsync();
        }

        // CommunityToolkit RelayCommand ile no-boilerplate komut.
        public IRelayCommand LoadRoomsCommand { get; }
        public IRelayCommand AddRoomCommand { get; }

        private async Task LoadRoomsAsync()
        {
            var roomList = await _roomService.GetAllRoomsAsync();
            Rooms.Clear();
            foreach (var room in roomList)
            {
                Rooms.Add(room);
            }
        }

        private async Task AddNewRoomAsync()
        { 
            var newRoom = new RoomDto
            {
                RoomName = "Oley be cok iyi oda",
                RoomCapacity = 3,
                RoomPrice = 750,
                RoomType = RoomType.Single,
                RoomStatus = RoomStatus.Available,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var created = await _roomService.CreateRoomAsync(newRoom);
            Rooms.Add(created);
        }
    }
}