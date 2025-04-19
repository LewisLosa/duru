using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel; // ObservableObject için eklendi
using CommunityToolkit.Mvvm.Input; // RelayCommand için eklendi
using System.Threading.Tasks; // Async komutlar için (opsiyonel)
using System.Windows; // MessageBox için (gerçek uygulamada DI ile soyutlanabilir)

namespace DataTest.UI.ViewModels.Pages
{
    public partial class ReservationViewModel : ObservableObject // ObservableObject'ten türetildi
    {
        [ObservableProperty] // INotifyPropertyChanged implementasyonunu otomatik yapar
        private int _reservationId;

        [ObservableProperty] private string _guestName;

        [ObservableProperty] private string _roomNumber;

        [ObservableProperty] private DateTime _checkInDate;

        [ObservableProperty] private DateTime _checkOutDate;

        [ObservableProperty] private string _status; // Örneğin: "Onaylandı", "Beklemede", "İptal Edildi"
    }

    // Rezervasyon Yönetimi ViewModel'ı (CommunityToolkit.Mvvm ile)
    public partial class ReservationManagementViewModel : ObservableObject // ObservableObject'ten türetildi
    {
        [ObservableProperty] // Koleksiyon için otomatik özellik
        private ObservableCollection<ReservationViewModel> _reservations;

        [ObservableProperty] // Seçili öğe için otomatik özellik
        [NotifyCanExecuteChangedFor(
            nameof(EditReservationCommand))] // Bu özellik değiştiğinde komutun CanExecute durumu güncellensin
        [NotifyCanExecuteChangedFor(nameof(DeleteReservationCommand))]
        // Bu özellik değiştiğinde komutun CanExecute durumu güncellensin
        private ReservationViewModel _selectedReservation;

        public ReservationManagementViewModel()
        {
            LoadSampleData();
            // Komutlar attribute ile tanımlandığı için InitializeCommands metodu kaldırıldı.
        }

        private void LoadSampleData()
        {
            Reservations = new ObservableCollection<ReservationViewModel>
            {
                new ReservationViewModel
                {
                    ReservationId = 1001, GuestName = "Ahmet Yılmaz", RoomNumber = "101",
                    CheckInDate = DateTime.Today.AddDays(1), CheckOutDate = DateTime.Today.AddDays(3),
                    Status = "Onaylandı"
                },
                new ReservationViewModel
                {
                    ReservationId = 1002, GuestName = "Ayşe Kaya", RoomNumber = "105",
                    CheckInDate = DateTime.Today.AddDays(2), CheckOutDate = DateTime.Today.AddDays(5),
                    Status = "Onaylandı"
                },
                new ReservationViewModel
                {
                    ReservationId = 1003, GuestName = "Mehmet Demir", RoomNumber = "202",
                    CheckInDate = DateTime.Today.AddDays(1), CheckOutDate = DateTime.Today.AddDays(2),
                    Status = "Beklemede"
                },
                new ReservationViewModel
                {
                    ReservationId = 1004, GuestName = "Fatma Çelik", RoomNumber = "301",
                    CheckInDate = DateTime.Today.AddDays(5), CheckOutDate = DateTime.Today.AddDays(10),
                    Status = "Onaylandı"
                },
                new ReservationViewModel
                {
                    ReservationId = 1005, GuestName = "Mustafa Arslan", RoomNumber = "203",
                    CheckInDate = DateTime.Today.AddDays(-2), CheckOutDate = DateTime.Today.AddDays(1),
                    Status = "İptal Edildi"
                },
            };
        }

        // Yeni Rezervasyon Ekleme Komutu
        [RelayCommand] // Bu metodu bir ICommand'a dönüştürür (AddReservationCommand)
        private void AddReservation()
        {
            // Örnek: Yeni rezervasyon ekle
            var newReservation = new ReservationViewModel
            {
                ReservationId = new Random().Next(1000, 9999), // Geçici ID
                GuestName = "Yeni Misafir",
                RoomNumber = "Boş Oda",
                CheckInDate = DateTime.Today.AddDays(7),
                CheckOutDate = DateTime.Today.AddDays(9),
                Status = "Beklemede"
            };
            Reservations.Add(newReservation);
            SelectedReservation = newReservation; // Yeni ekleneni seçili yap
            // Gerçek uygulamada burada bir dialog/pencere açılır.
            MessageBox.Show("Örnek: Yeni rezervasyon eklendi.");
        }

        // Rezervasyon Düzenleme Komutu
        // CanExecute metodu otomatik olarak kontrol edilir (_selectedReservation null değilse çalışır)
        [RelayCommand(CanExecute = nameof(CanEditOrDeleteReservation))]
        private void EditReservation(ReservationViewModel reservation) // CommandParameter otomatik olarak buraya gelir
        {
            if (reservation == null) return; // Güvenlik kontrolü (CanExecute olsa da)

            // Örnek: Düzenleme işlemi (Dialog açılabilir)
            reservation.Status = "Düzenlendi (Örnek)";
            MessageBox.Show($"Rezervasyon Düzenleniyor: ID {reservation.ReservationId}");
        }

        // Rezervasyon Silme Komutu
        // CanExecute metodu otomatik olarak kontrol edilir (_selectedReservation null değilse çalışır)
        [RelayCommand(CanExecute = nameof(CanEditOrDeleteReservation))]
        private void
            DeleteReservation(ReservationViewModel reservation) // CommandParameter otomatik olarak buraya gelir
        {
            if (reservation == null) return;

            // Onay isteyebilirsiniz
            var result = MessageBox.Show(
                $"'{reservation.GuestName}' adlı misafirin {reservation.ReservationId} ID'li rezervasyonunu silmek istediğinize emin misiniz?",
                "Rezervasyon Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Reservations.Remove(reservation);
                MessageBox.Show("Rezervasyon silindi.");
            }
        }

        // Düzenleme ve Silme için CanExecute metodu
        private bool CanEditOrDeleteReservation()
        {
            return SelectedReservation != null;
        }

        // Rezervasyonları Yenileme Komutu
        [RelayCommand]
        private void RefreshReservations()
        {
            MessageBox.Show("Rezervasyonlar yenileniyor...");
            // Gerçek uygulamada veriyi kaynaktan tekrar çeker
            LoadSampleData();
        }

        // Eğer async işlemler yapacaksanız:
        /*
        [RelayCommand]
        private async Task RefreshReservationsAsync()
        {
            // IsBusy = true; // Yükleniyor göstergesi için
            await Task.Delay(1000); // Örnek async işlem (veri çekme vs.)
            LoadSampleData();
            // IsBusy = false;
             MessageBox.Show("Rezervasyonlar asenkron olarak yenilendi.");
        }
        */
    }
}