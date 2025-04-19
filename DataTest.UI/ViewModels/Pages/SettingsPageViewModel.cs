using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // String listesi yerine ObservableCollection gerekebilir mi? Hayır, Liste yeterli.
using System.Linq;
using System.Threading.Tasks;
using System.Windows; // Application.Current için
// using System.Windows.Media; // Artık Brush kullanmıyoruz
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlzEx.Theming; // MahApps Tema Yönetimi için
using MahApps.Metro.Controls.Dialogs; // DialogCoordinator için

namespace DataTest.UI.ViewModels.Pages
{
    // Ayarlar Sayfası ViewModel'ı (Varsayılan Tema ile)
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private bool _isInitializing = true; // Başlangıç yüklemesinde IsDirty'nin tetiklenmesini engellemek için

        // --- Görünüm Ayarları (Sadece İsimler) ---
        [ObservableProperty] private string _selectedBaseTheme;
        [ObservableProperty] private List<string> _availableBaseThemes;
        [ObservableProperty] private string _selectedAccentColor;
        [ObservableProperty] private List<string> _availableAccentColors;


        // --- Diğer Ayarlar ---
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _hotelName;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _hotelAddress;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _hotelPhone;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _defaultCurrency;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private TimeSpan? _defaultCheckInTime;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private TimeSpan? _defaultCheckOutTime;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private decimal _vatRate;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _invoicePrefix;
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _defaultReservationStatus;
        public List<string> ReservationStatuses { get; } = new List<string> { "Onaylandı", "Beklemede", "İptal Edildi" };
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsDirty))] private string _cancellationPolicy;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))] private bool _isDirty;


        public SettingsPageViewModel(IDialogCoordinator dialogCoordinator = null)
        {
            _dialogCoordinator = dialogCoordinator ?? DialogCoordinator.Instance;

            // Sabit İsim Listelerini Oluştur
            CreateHardcodedThemeNameList();
            CreateHardcodedAccentNameList();

            // Varsayılanları Ata (Algılamaya Gerek Yok)
            // _selectedBaseTheme = "Light"; // Doğrudan atama OnPropertyChanged'i tetikler
            // _selectedAccentColor = "Blue"; // Bu yüzden özellikler üzerinden gidelim
            SelectedBaseTheme = "Light";
            SelectedAccentColor = "Blue";
             // ApplyThemeChange() metodu OnPropertyChanged içinde çağrılacak.

            LoadSettings(); // Kayıtlı ayarları yükle (simülasyon)

            _isInitializing = false; // Başlangıç yüklemesi bitti
            IsDirty = false; // Başlangıçta değişiklik yok (LoadSettings sonrası)
        }
        public SettingsPageViewModel() : this(DialogCoordinator.Instance) { }


        private void CreateHardcodedThemeNameList()
        {
            AvailableBaseThemes = new List<string> { "Light", "Dark" };
        }

        private void CreateHardcodedAccentNameList()
        {
            AvailableAccentColors = new List<string>
            {
                "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald",
                "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta",
                "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve",
                "Taupe", "Sienna"
            };
        }

        private void LoadSettings()
        {
            // --- AYARLARI YÜKLEME İŞLEMİ (Simülasyon) ---
            // Kayıtlı tema/renk varsa burada SelectedBaseTheme ve SelectedAccentColor'ı güncelleyebiliriz.
            // Şimdilik varsayılan (Light/Blue) kalıyor, diğer ayarlar yükleniyor.
            HotelName = "Harika Otel A.Ş. (Varsayılan)";
            HotelAddress = "Örnek Mah. Test Cad. No:123 Arnavutköy/İstanbul (Varsayılan)";
            HotelPhone = "+90 212 555 1111";
            DefaultCurrency = "TRY";
            DefaultCheckInTime = new TimeSpan(14, 0, 0);
            DefaultCheckOutTime = new TimeSpan(12, 0, 0);
            VatRate = 18.0m;
            InvoicePrefix = "HS-";
            DefaultReservationStatus = "Onaylandı";
            CancellationPolicy = "Standart iptal politikası geçerlidir.";
            System.Diagnostics.Debug.WriteLine("Ayarlar yüklendi (varsayılan tema ile).");
            // ------------------------------------------
        }

        // Temayı Uygula Metodu
        private void ApplyThemeChange()
        {
            // Başlatma sırasında veya geçersiz seçimlerde çalıştırma
            if (_isInitializing || string.IsNullOrEmpty(SelectedBaseTheme) || string.IsNullOrEmpty(SelectedAccentColor)) return;

            try
            {
                var app = Application.Current;
                if (app != null)
                {
                    ThemeManager.Current.ChangeTheme(app, SelectedBaseTheme, SelectedAccentColor);
                    System.Diagnostics.Debug.WriteLine($"Tema değiştirildi: {SelectedBaseTheme}.{SelectedAccentColor}");
                    IsDirty = true; // Kullanıcı seçim yaptığı için kirli olarak işaretle
                }
            }
            catch (Exception ex) { _dialogCoordinator.ShowMessageAsync(this, "Tema Hatası", $"Tema değiştirilirken bir hata oluştu: {ex.Message}"); }
        }

        // Kaydet Komutu
        [RelayCommand(CanExecute = nameof(CanSaveSettings))]
        private async Task SaveSettings()
        {
            var result = await _dialogCoordinator.ShowMessageAsync(this, "Ayarları Kaydet", "Yapılan değişiklikleri kaydetmek istediğinize emin misiniz?",
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Evet, Kaydet", NegativeButtonText = "İptal" });
            if (result != MessageDialogResult.Affirmative) return;

            var progress = await _dialogCoordinator.ShowProgressAsync(this, "Kaydediliyor...", "Ayarlarınız kaydediliyor...");
            progress.SetIndeterminate(); await Task.Delay(1000);

            // --- AYARLARI KAYDETME İŞLEMİ (Simülasyon) ---
            System.Diagnostics.Debug.WriteLine($"Kaydedilen Tema: {SelectedBaseTheme}.{SelectedAccentColor}");
            // Gerçek kaydetme kodları buraya gelir.
            // SettingsService.SaveTheme(SelectedBaseTheme, SelectedAccentColor);
            // SettingsService.SaveHotelName(HotelName); ... vb
            // ------------------------------------------

            IsDirty = false;
            await progress.CloseAsync();
            await _dialogCoordinator.ShowMessageAsync(this, "Başarılı", "Ayarlar başarıyla kaydedildi.");
        }
        private bool CanSaveSettings() => IsDirty;

        // Özellik Değişikliklerini İzleme
        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // Başlatma tamamlandıktan sonra
            if (!_isInitializing)
            {
                // Tema veya renk seçimi değiştiğinde anında uygula
                if (e.PropertyName == nameof(SelectedBaseTheme) || e.PropertyName == nameof(SelectedAccentColor))
                {
                    ApplyThemeChange();
                }
                // IsDirty dışındaki diğer özellikler değiştiğinde kirli olarak işaretle
                else if (e.PropertyName != nameof(IsDirty))
                {
                    IsDirty = true;
                }
            }
        }
    }
}