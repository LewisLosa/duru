using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DataTest.UI.ViewModels.Pages
{
    // Örnek Misafir Modeli (CommunityToolkit.Mvvm ile)
    public partial class GuestViewModel : ObservableObject
    {
        [ObservableProperty] private int _guestId;

        [ObservableProperty] private string _firstName;

        [ObservableProperty] private string _lastName;

        [ObservableProperty] private string _email;

        [ObservableProperty] private string _phoneNumber;

        [ObservableProperty] private string _country;
    }

    // Misafir Yönetimi ViewModel'ı (CommunityToolkit.Mvvm ile)
    public partial class GuestManagementViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<GuestViewModel> _guests;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditGuestCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteGuestCommand))]
        private GuestViewModel _selectedGuest;

        public GuestManagementViewModel()
        {
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            Guests = new ObservableCollection<GuestViewModel>
            {
                new GuestViewModel
                {
                    GuestId = 2001, FirstName = "Ali", LastName = "Veli", Email = "ali.veli@example.com",
                    PhoneNumber = "555-1234", Country = "Türkiye"
                },
                new GuestViewModel
                {
                    GuestId = 2002, FirstName = "Zeynep", LastName = "Öztürk", Email = "zeynep.ozturk@example.com",
                    PhoneNumber = "555-5678", Country = "Türkiye"
                },
                new GuestViewModel
                {
                    GuestId = 2003, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com",
                    PhoneNumber = "123-456-7890", Country = "USA"
                },
                new GuestViewModel
                {
                    GuestId = 2004, FirstName = "Maria", LastName = "Garcia", Email = "maria.garcia@example.com",
                    PhoneNumber = "987-654-3210", Country = "Spain"
                },
                new GuestViewModel
                {
                    GuestId = 2005, FirstName = "Hans", LastName = "Müller", Email = "hans.muller@example.com",
                    PhoneNumber = "111-222-3333", Country = "Germany"
                },
            };
        }

        [RelayCommand]
        private void AddGuest()
        {
            var newGuest = new GuestViewModel
            {
                GuestId = new Random().Next(2000, 3000), // Geçici ID
                FirstName = "Yeni",
                LastName = "Misafir",
                Email = "yeni@example.com",
                PhoneNumber = "555-0000",
                Country = "Bilinmiyor"
            };
            Guests.Add(newGuest);
            SelectedGuest = newGuest;
            MessageBox.Show("Örnek: Yeni misafir eklendi.");
        }

        [RelayCommand(CanExecute = nameof(CanEditOrDeleteGuest))]
        private void EditGuest(GuestViewModel guest)
        {
            if (guest == null) return;

            guest.Country = "Düzenlendi (Örnek)"; // Örnek düzenleme
            MessageBox.Show($"Misafir Düzenleniyor: {guest.FirstName} {guest.LastName}");
        }

        [RelayCommand(CanExecute = nameof(CanEditOrDeleteGuest))]
        private void DeleteGuest(GuestViewModel guest)
        {
            if (guest == null) return;

            var result = MessageBox.Show(
                $"'{guest.FirstName} {guest.LastName}' adlı misafiri silmek istediğinize emin misiniz?",
                "Misafir Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Guests.Remove(guest);
                MessageBox.Show("Misafir silindi.");
            }
        }

        private bool CanEditOrDeleteGuest()
        {
            return SelectedGuest != null;
        }

        [RelayCommand]
        private void RefreshGuests()
        {
            MessageBox.Show("Misafir listesi yenileniyor...");
            LoadSampleData();
        }
    }
}