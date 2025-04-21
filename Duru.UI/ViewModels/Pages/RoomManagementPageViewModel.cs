// Duru.UI/ViewModels/Pages/RoomManagementPageViewModel.cs

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Duru.Data.Enums;
using Duru.Data.Models;
using Duru.Data.Services;
using MahApps.Metro.Controls.Dialogs;
// For AsyncRelayCommand
// Use the actual data model
// Use the service

// For DialogCoordinator

namespace Duru.UI.ViewModels.Pages;

// Note: The nested RoomViewModel might need adjustments or removal if Duru.Data.Models.Room is used directly
// For this example, let's assume you want to keep a separate VM for the UI list item.
public partial class RoomListItemViewModel : ObservableObject // Renamed from RoomViewModel to avoid conflict
{
    [ObservableProperty] private string _floor;
    [ObservableProperty] private int _id; // Keep track of the actual ID
    [ObservableProperty] private decimal _pricePerNight;
    [ObservableProperty] private string _roomNumber;
    [ObservableProperty] private string _roomType; // Could map from enum
    [ObservableProperty] private string _status; // Could map from enum

    // Add a constructor for mapping
    public RoomListItemViewModel(Room room)
    {
        _id = room.Id;
        _roomNumber = room.RoomNumber;
        _floor = room.Floor;
        _roomType = room.RoomType.ToString(); // Simple mapping for now
        _status = room.Status.ToString(); // Simple mapping for now
        _pricePerNight = room.PricePerNight;
    }
    public RoomListItemViewModel()
    {
    }
}

public partial class RoomManagementViewModel : ObservableObject
{
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly IRoomService _roomService;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private ObservableCollection<RoomListItemViewModel> _rooms; // Use the list item VM

    // Add filters/search properties if needed, similar to your original code
    [ObservableProperty] private string _searchText = string.Empty;

    [ObservableProperty] private RoomListItemViewModel? _selectedRoom; // Make nullable


    public RoomManagementViewModel(IRoomService roomService, IDialogCoordinator dialogCoordinator)
    {
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
        _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
        _rooms = new ObservableCollection<RoomListItemViewModel>();

        // Load data when the ViewModel is created
        // Using Fire and Forget - consider a better approach like an async init method or framework event
        _ = LoadRoomsAsync();
    }

    [RelayCommand] // Use RelayCommand for synchronous search/filter if it doesn't hit DB
    private void ApplyFilter()
    {
        // Re-apply filtering logic based on _searchText etc. on the _rooms collection
        // This should ideally use CollectionViewSource for better performance in WPF
        // For simplicity, let's just reload all for now if search text changes
        _ = LoadRoomsAsync(); // Reload if filtering is complex or needs DB
    }

    // Watch for changes in SearchText to trigger filtering/reload
    partial void OnSearchTextChanged(string value)
    {
        // Debounce this in a real app to avoid too many reloads
        ApplyFilterCommand.Execute(null);
    }


    [RelayCommand] // Use AsyncRelayCommand for DB operations
    private async Task LoadRoomsAsync()
    {
        IsLoading = true;
        Rooms.Clear(); // Clear existing items
        try
        {
            var roomsFromDb = await _roomService.GetAllRoomsAsync();

            // Apply local filtering if needed
            if (!string.IsNullOrWhiteSpace(SearchText))
                roomsFromDb = roomsFromDb.Where(
                    r => r.RoomNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                         r.Floor.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                         r.RoomType.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                         r.Status.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            // Add sorting if needed

            foreach (var room in roomsFromDb.OrderBy(r => r.RoomNumber)) // Example sorting
                Rooms.Add(new RoomListItemViewModel(room)); // Map to the list item VM
        }
        catch (Exception ex)
        {
            // Use DialogCoordinator for errors
            await _dialogCoordinator.ShowMessageAsync(
                this, "Error Loading Rooms", $"Could not load room data: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDeleteRoom))]
    private async Task EditRoomAsync()
    {
        if (SelectedRoom == null) return;

        // In a real app, open a dialog/window to edit the selected room
        // For now, just show a message and maybe simulate an update
        await _dialogCoordinator.ShowMessageAsync(
            this, "Edit Room",
            $"Editing Room ID: {SelectedRoom.Id} ({SelectedRoom.RoomNumber}). (Not implemented yet)");

        // Example simulation: Fetch the actual room, modify, save
        // var roomToUpdate = await _roomService.GetRoomByIdAsync(SelectedRoom.Id);
        // if (roomToUpdate != null) {
        //    roomToUpdate.Status = Data.Enums.RoomStatus.Maintenance; // Example change
        //    await _roomService.UpdateRoomAsync(roomToUpdate);
        //    await LoadRoomsAsync(); // Refresh list
        // }
    }

    [RelayCommand]
    private async Task AddRoomAsync()
    {
        // In a real app, open a dialog/window to get new room details
        var newRoomData = new Room // Create a dummy room for now
        {
            RoomNumber = $"New{new Random().Next(100, 999)}",
            Floor = "1",
            RoomType = RoomType.Single,
            Status = RoomStatus.Available,
            PricePerNight = 150,
        };

        try
        {
            await _roomService.AddRoomAsync(newRoomData);
            await LoadRoomsAsync(); // Refresh list
            await _dialogCoordinator.ShowMessageAsync(this, "Success", $"Room '{newRoomData.RoomNumber}' added.");
        }
        catch (Exception ex)
        {
            await _dialogCoordinator.ShowMessageAsync(this, "Error Adding Room", $"Could not add room: {ex.Message}");
        }
    }


    [RelayCommand(CanExecute = nameof(CanEditOrDeleteRoom))]
    private async Task DeleteRoomAsync()
    {
        if (SelectedRoom == null) return;

        var result = await _dialogCoordinator.ShowMessageAsync(
            this, "Confirm Delete",
            $"Are you sure you want to delete room '{SelectedRoom.RoomNumber}' (ID: {SelectedRoom.Id})?",
            MessageDialogStyle.AffirmativeAndNegative,
            new MetroDialogSettings { AffirmativeButtonText = "Yes, Delete", NegativeButtonText = "Cancel" });

        if (result == MessageDialogResult.Affirmative)
        {
            IsLoading = true;
            try
            {
                await _roomService.DeleteRoomAsync(SelectedRoom.Id);
                await LoadRoomsAsync(); // Refresh the list
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(
                    this, "Error Deleting Room", $"Could not delete room: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    // CanExecute condition for commands that require a selection
    private bool CanEditOrDeleteRoom()
    {
        return SelectedRoom != null && !IsLoading;
    }

    // Notify CanExecuteChanged when SelectedRoom changes
    partial void OnSelectedRoomChanged(RoomListItemViewModel? value)
    {
        EditRoomCommand.NotifyCanExecuteChanged();
        DeleteRoomCommand.NotifyCanExecuteChanged();
    }
    // Notify CanExecuteChanged when IsLoading changes
    partial void OnIsLoadingChanged(bool value)
    {
        EditRoomCommand.NotifyCanExecuteChanged();
        DeleteRoomCommand.NotifyCanExecuteChanged();
    }
}