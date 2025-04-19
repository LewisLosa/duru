using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;

namespace DataTest.UI.ViewModels.Pages;

// Sadece kategori bilgisini tutan ViewModel
public partial class IntegrationCategoryViewModel : ObservableObject
{
    [ObservableProperty] private string _id; // Kategoriyi benzersiz olarak tanımlamak için (örn: "booking_platforms")

    [ObservableProperty] private string _title;

    [ObservableProperty] private string _description;

    [ObservableProperty] private PackIconMaterialKind _iconKind;
}

    public partial class IntegrationsViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        [ObservableProperty]
        private ObservableCollection<IntegrationCategoryViewModel> _integrationCategories;

        public IntegrationsViewModel(IDialogCoordinator dialogCoordinator = null)
        {
            _dialogCoordinator = dialogCoordinator ?? DialogCoordinator.Instance;
            LoadIntegrationCategories();
        }

        // DI container olmadan test/tasarım zamanı için
        public IntegrationsViewModel() : this(DialogCoordinator.Instance) { }


        private void LoadIntegrationCategories()
        {
             // IntegrationCategoryViewModel listesini doldurma kodu (önceki cevapla aynı)
             IntegrationCategories = new ObservableCollection<IntegrationCategoryViewModel>
            {
                 new IntegrationCategoryViewModel { Id = "booking_platforms", Title = "Rezervasyon Platformları", Description = "Booking.com, Expedia, Airbnb gibi platformlarla entegrasyon.", IconKind = PackIconMaterialKind.CalendarMultiselect },
                 new IntegrationCategoryViewModel { Id = "payment_systems", Title = "Ödeme Sistemleri", Description = "Online ödeme ağ geçitleri (Stripe, PayPal) ve POS cihazları.", IconKind = PackIconMaterialKind.CreditCardSettingsOutline },
                 new IntegrationCategoryViewModel { Id = "pos_accounting", Title = "POS ve Muhasebe", Description = "Restoran POS sistemleri ve muhasebe yazılımları ile veri akışı.", IconKind = PackIconMaterialKind.ReceiptTextOutline },
                 new IntegrationCategoryViewModel { Id = "keycard_systems", Title = "Anahtar Kart Sistemleri", Description = "Oda anahtar kartı sistemleri (VingCard, Salto vb.) ile entegrasyon.", IconKind = PackIconMaterialKind.CreditCardChipOutline },
                 new IntegrationCategoryViewModel { Id = "api_automation", Title = "API ve Otomasyon", Description = "Özel geliştirmeler ve otomasyonlar için API erişimi.", IconKind = PackIconMaterialKind.Api },
                 new IntegrationCategoryViewModel { Id = "security_auth", Title = "Güvenlik ve Kimlik Doğrulama", Description = "Kullanıcı yetkilendirme ve güvenlik sistemleri (SSO, LDAP vb.).", IconKind = PackIconMaterialKind.ShieldLockOutline },
                 new IntegrationCategoryViewModel { Id = "reporting_analytics", Title = "Raporlama ve Analitik Araçları", Description = "Dış raporlama araçlarına (Power BI vb.) veri sağlama.", IconKind = PackIconMaterialKind.ChartDonut },
                 new IntegrationCategoryViewModel { Id = "communication_systems", Title = "İletişim Sistemleri", Description = "WhatsApp, SMS, E-posta gönderim servisleri ile entegrasyon.", IconKind = PackIconMaterialKind.ForumOutline }
            };
        }

        [RelayCommand]
        private async Task ShowIntegrationSettings(IntegrationCategoryViewModel category)
        {
            if (category == null) return;

            string dialogTitle = $"{category.Title} Ayarları";
            string message = $"'{category.Title}' entegrasyonu için ayarlar burada yapılandırılacaktır.\n\n{category.Description}";

            // Örnek: API ve Otomasyon için farklı bir mesaj gösterelim
            if (category.Id == "api_automation")
            {
                message = "API erişimi ve otomasyon kuralları bu bölümden yönetilecektir. Detaylı dokümantasyon için 'Yardım' bölümüne bakınız.";
            }
            // Örnek: Ödeme Sistemleri için Input Dialog deneyelim (tek bir API key sormak için)
            else if (category.Id == "payment_systems")
            {
                 var apiKey = await _dialogCoordinator.ShowInputAsync(this,
                                                                     dialogTitle,
                                                                     "Lütfen kullanmak istediğiniz ana ödeme sistemi API anahtarını girin (Örn: Stripe Secret Key):");

                 if (string.IsNullOrWhiteSpace(apiKey))
                 {
                     await _dialogCoordinator.ShowMessageAsync(this, dialogTitle, "API anahtarı girilmedi veya iptal edildi.");
                 }
                 else
                 {
                     // API anahtarını kaydetme işlemi burada yapılır.
                     await _dialogCoordinator.ShowMessageAsync(this, dialogTitle, $"'{apiKey.Substring(0, 5)}...' ile başlayan API anahtarı kaydedildi (Örnek).");
                 }
                 return; // ShowMessageAsync'i tekrar çağırmamak için buradan çık
            }


            // Diğer kategoriler için genel bilgi mesajı göster
            await _dialogCoordinator.ShowMessageAsync(this, dialogTitle, message);

            // Kullanıcı dialogu kapattıktan sonra yapılacaklar (eğer gerekirse)
        }
    }
