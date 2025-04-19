using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows; // MessageBox için
using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace DataTest.UI.ViewModels.Pages;

// Örnek Ödeme/Fatura Modeli
public partial class PaymentViewModel : ObservableObject
{
    [ObservableProperty] private int _invoiceId; // Fatura veya Ödeme ID

    [ObservableProperty] private int? _reservationId; // İlişkili Rezervasyon ID (opsiyonel)

    [ObservableProperty] private string _guestName;

    [ObservableProperty] private string _roomNumber;

    [ObservableProperty] private decimal _totalAmount; // Toplam Tutar

    [ObservableProperty] private decimal _amountPaid; // Ödenen Tutar

    [ObservableProperty] private DateTime _paymentDate; // Son Ödeme/İşlem Tarihi

    [ObservableProperty] private string _paymentMethod; // Nakit, Kredi Kartı, Havale vb.

    [ObservableProperty] private string _status; // Ödendi, Kısmi Ödendi, Ödenmedi, İade Edildi

    // Hesaplanan Özellik (Örnek: Kalan Bakiye)
    public decimal BalanceDue => TotalAmount - AmountPaid;

    // Status özelliğine bağlı olarak DataGrid'de farklı renkler kullanmak için
    // bir property daha ekleyebiliriz veya Converter kullanabiliriz.
}

public partial class PaymentManagementViewModel : ObservableObject
{
    // Asıl liste (filtrelenmemiş)
    private List<PaymentViewModel> _allPayments;

    [ObservableProperty]
    private ObservableCollection<PaymentViewModel> _payments; // DataGrid'e bağlanacak filtrelenmiş liste

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ViewPaymentDetailsCommand))]
    [NotifyCanExecuteChangedFor(nameof(RecordPaymentCommand))]
    [NotifyCanExecuteChangedFor(nameof(IssueRefundCommand))]
    private PaymentViewModel _selectedPayment;

    // Filtreleme Özellikleri
    [ObservableProperty]
    private DateTime? _filterStartDate = DateTime.Today.AddMonths(-1); // Varsayılan başlangıç tarihi

    [ObservableProperty] private DateTime? _filterEndDate = DateTime.Today; // Varsayılan bitiş tarihi

    [ObservableProperty] private string _filterGuestName;

    [ObservableProperty] private string _selectedStatusFilter = "Tümü"; // Varsayılan filtre

    public List<string> StatusFilters { get; } = new List<string>
        { "Tümü", "Ödendi", "Kısmi Ödendi", "Ödenmedi", "İade Edildi" };

    public PaymentManagementViewModel()
    {
        LoadSampleData();
        ApplyFilter(); // Başlangıçta filtreyi uygula
    }

    private void LoadSampleData()
    {
        // Gerçek uygulamada bu veriler veritabanından veya servisten gelir
        _allPayments = new List<PaymentViewModel>
        {
            new PaymentViewModel
            {
                InvoiceId = 3001, ReservationId = 1001, GuestName = "Ahmet Yılmaz", RoomNumber = "101",
                TotalAmount = 450.00m, AmountPaid = 450.00m, PaymentDate = DateTime.Today.AddDays(-1),
                PaymentMethod = "Kredi Kartı", Status = "Ödendi"
            },
            new PaymentViewModel
            {
                InvoiceId = 3002, ReservationId = 1002, GuestName = "Ayşe Kaya", RoomNumber = "105",
                TotalAmount = 750.50m, AmountPaid = 750.50m, PaymentDate = DateTime.Today.AddDays(-2),
                PaymentMethod = "Nakit", Status = "Ödendi"
            },
            new PaymentViewModel
            {
                InvoiceId = 3003, ReservationId = 1003, GuestName = "Mehmet Demir", RoomNumber = "202",
                TotalAmount = 200.00m, AmountPaid = 100.00m, PaymentDate = DateTime.Today, PaymentMethod = "Nakit",
                Status = "Kısmi Ödendi"
            },
            new PaymentViewModel
            {
                InvoiceId = 3004, ReservationId = 1004, GuestName = "Fatma Çelik", RoomNumber = "301",
                TotalAmount = 1200.75m, AmountPaid = 0m, PaymentDate = DateTime.Today.AddDays(-5), PaymentMethod = "-",
                Status = "Ödenmedi"
            },
            new PaymentViewModel
            {
                InvoiceId = 3005, ReservationId = 1005, GuestName = "Mustafa Arslan", RoomNumber = "203",
                TotalAmount = 300.00m, AmountPaid = 0m, PaymentDate = DateTime.Today.AddDays(-10),
                PaymentMethod = "Kredi Kartı", Status = "İade Edildi"
            },
            new PaymentViewModel
            {
                InvoiceId = 3006, ReservationId = 1006, GuestName = "Zeynep Öztürk", RoomNumber = "201",
                TotalAmount = 550.00m, AmountPaid = 550.00m, PaymentDate = DateTime.Today.AddDays(-15),
                PaymentMethod = "Havale", Status = "Ödendi"
            }
        };
        // Başlangıçta filtrelenmiş liste, tüm verileri içerir
        Payments = new ObservableCollection<PaymentViewModel>(_allPayments);
    }

    [RelayCommand]
    private void ApplyFilter()
    {
        IEnumerable<PaymentViewModel> filtered = _allPayments;

        if (FilterStartDate.HasValue)
        {
            filtered = filtered.Where(p => p.PaymentDate.Date >= FilterStartDate.Value.Date);
        }

        if (FilterEndDate.HasValue)
        {
            filtered = filtered.Where(p => p.PaymentDate.Date <= FilterEndDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(FilterGuestName))
        {
            filtered = filtered.Where(p => p.GuestName.Contains(FilterGuestName, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedStatusFilter != "Tümü")
        {
            filtered = filtered.Where(p => p.Status == SelectedStatusFilter);
        }

        Payments = new ObservableCollection<PaymentViewModel>(filtered.ToList());
    }

    [RelayCommand]
    private void RefreshPayments()
    {
        MessageBox.Show("Ödeme listesi yenileniyor...");
        LoadSampleData(); // Örnek verileri yeniden yükle
        ApplyFilter(); // Filtreyi tekrar uygula
    }

    // Seçili bir ödeme olduğunda çalışacak komutlar için CanExecute metodu
    private bool CanExecutePaymentAction()
    {
        return SelectedPayment != null;
    }

    [RelayCommand(CanExecute = nameof(CanExecutePaymentAction))]
    private void ViewPaymentDetails(PaymentViewModel payment)
    {
        if (payment == null) return;
        // Gerçek uygulamada detay penceresi açılır
        MessageBox.Show(
            $"Fatura Detayı Görüntüleniyor: ID {payment.InvoiceId}\nMisafir: {payment.GuestName}\nTutar: {payment.TotalAmount:C}");
    }

    [RelayCommand(CanExecute = nameof(CanExecutePaymentAction))]
    private void RecordPayment(PaymentViewModel payment)
    {
        if (payment == null || payment.Status == "Ödendi" || payment.Status == "İade Edildi")
        {
            MessageBox.Show(
                "Bu fatura için ödeme kaydedilemez (Zaten Ödendi veya İade Edildi).", "Uyarı", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        // Gerçek uygulamada ödeme alma penceresi açılır
        // Örnek: Tutarın tamamı ödensin
        payment.AmountPaid = payment.TotalAmount;
        payment.Status = "Ödendi";
        payment.PaymentDate = DateTime.Now; // Ödeme tarihini güncelle
        payment.PaymentMethod = "Kredi Kartı (Örnk)"; // Ödeme yöntemini güncelle
        MessageBox.Show($"Ödeme Kaydedildi: ID {payment.InvoiceId}\nYeni Durum: {payment.Status}");
        // Liste görünümünü güncellemek için PropertyChanged tetiklenmeli,
        // ObservableObject bunu otomatik yapar ancak koleksiyonun kendisi değişmediği için
        // DataGrid'deki satırın güncellemesi için ek mekanizma gerekebilir (veya tüm listeyi yenilemek)
        ApplyFilter(); // Filtreyi tekrar uygulayarak listeyi yenileyelim
    }

    [RelayCommand(CanExecute = nameof(CanExecutePaymentAction))]
    private void IssueRefund(PaymentViewModel payment)
    {
        if (payment == null || payment.Status != "Ödendi") // Sadece ödenmiş faturalar iade edilebilir (örnek kural)
        {
            MessageBox.Show(
                "Sadece 'Ödendi' durumundaki faturalar için iade işlemi yapılabilir.", "Uyarı", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        // Gerçek uygulamada iade penceresi/onayı açılır
        var result = MessageBox.Show(
            $"{payment.InvoiceId} ID'li fatura için {payment.AmountPaid:C} tutarında iade yapmak istediğinize emin misiniz?",
            "İade Onayı", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            payment.Status = "İade Edildi";
            payment.AmountPaid = 0; // İade edildiği için ödenen tutarı sıfırla (veya negatif yap)
            payment.PaymentDate = DateTime.Now; // İşlem tarihini güncelle
            MessageBox.Show($"İade İşlemi Yapıldı: ID {payment.InvoiceId}\nYeni Durum: {payment.Status}");
            ApplyFilter(); // Listeyi yenile
        }
    }
}