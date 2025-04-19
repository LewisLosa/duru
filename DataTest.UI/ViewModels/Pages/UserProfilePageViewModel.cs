using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Windows; // PasswordBox için veya DI

namespace DataTest.UI.ViewModels.Pages
{
    public partial class UserProfileViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private bool _isInitializing = true;
        private string _originalFullName; // Değişiklik kontrolü için
        private string _originalEmail;
        private string _originalPhoneNumber;

        // --- Görüntülenen/Düzenlenen Bilgiler ---
        [ObservableProperty] private string _username = "eyups"; // Değiştirilemez (örnek)
        [ObservableProperty] private string _role = "Administrator"; // Değiştirilemez (örnek)
        [ObservableProperty] private DateTime _lastLogin = DateTime.Now.AddHours(-2); // Örnek

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsDirty))]
        private string _fullName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsDirty))]
        private string _email;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsDirty))]
        private string _phoneNumber;


        // --- Şifre Değiştirme Alanları ---
        // Not: PasswordBox'ları MVVM ile bağlamak ek teknikler gerektirebilir.
        // Güvenlik nedeniyle şifreleri string olarak ViewModel'da tutmak önerilmez.
        // Bu örnekte basitlik adına string kullanıyoruz, ancak gerçek uygulamada
        // SecureString veya direkt code-behind event handling daha güvenlidir.
        [ObservableProperty] private string _currentPassword = "";
        [ObservableProperty] private string _newPassword = "";
        [ObservableProperty] private string _confirmNewPassword = "";

        [ObservableProperty] private bool _isPasswordChangeVisible = false;


        // --- Durum ---
        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))] private bool _isDirty;


        public UserProfileViewModel(IDialogCoordinator dialogCoordinator = null)
        {
            _dialogCoordinator = dialogCoordinator ?? DialogCoordinator.Instance;
            LoadUserProfile();
            _isInitializing = false;
        }
        public UserProfileViewModel() : this(DialogCoordinator.Instance) { }


        private void LoadUserProfile()
        {
            // --- OTURUM AÇMIŞ KULLANICI BİLGİLERİNİ YÜKLEME (Simülasyon) ---
            // Gerçek uygulamada bu bilgiler bir Authentication Servisinden veya veritabanından gelir.
            _username = "eyups"; // Sabit kalsın
            _role = "Administrator"; // Sabit kalsın
            FullName = "Eyüp Şengöz";
            Email = "eyup.sengoz@example.com";
            PhoneNumber = "+90 555 123 4567";
            _lastLogin = DateTime.Now.AddMinutes(-new Random().Next(5, 120)); // Rastgele son giriş

            // Değişiklik kontrolü için orijinal değerleri sakla
            _originalFullName = FullName;
            _originalEmail = Email;
            _originalPhoneNumber = PhoneNumber;
            // ------------------------------------------

            IsDirty = false; // Yükleme sonrası temiz durumda
        }

        [RelayCommand]
        private void TogglePasswordChangeVisibility()
        {
            IsPasswordChangeVisible = !IsPasswordChangeVisible;
            // Şifre alanlarını temizle
            CurrentPassword = "";
            NewPassword = "";
            ConfirmNewPassword = "";
        }

        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        private async Task SaveChanges()
        {
            // E-posta format kontrolü (basit)
            if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Geçersiz E-posta", "Lütfen geçerli bir e-posta adresi girin.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                return;
            }

             var result = await _dialogCoordinator.ShowMessageAsync(this, "Değişiklikleri Kaydet", "Profil bilgilerinizde yapılan değişiklikleri kaydetmek istediğinize emin misiniz?",
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Evet, Kaydet", NegativeButtonText="İptal"});

            if (result != MessageDialogResult.Affirmative) return;

             var progress = await _dialogCoordinator.ShowProgressAsync(this, "Kaydediliyor...", "Profil bilgileriniz güncelleniyor...");
             progress.SetIndeterminate();
             await Task.Delay(1000); // Simülasyon

            // --- PROFİL BİLGİLERİNİ KAYDETME (Simülasyon) ---
            // Gerçek uygulamada Authentication Servisi veya veritabanı güncellenir.
            System.Diagnostics.Debug.WriteLine($"Kaydedilen Ad Soyad: {FullName}");
            System.Diagnostics.Debug.WriteLine($"Kaydedilen E-posta: {Email}");
            System.Diagnostics.Debug.WriteLine($"Kaydedilen Telefon: {PhoneNumber}");
             _originalFullName = FullName; // Orijinal değerleri güncelle
             _originalEmail = Email;
             _originalPhoneNumber = PhoneNumber;
            // ------------------------------------------

             IsDirty = false; // Kaydedildiği için temiz
            await progress.CloseAsync();
            await _dialogCoordinator.ShowMessageAsync(this, "Başarılı", "Profil bilgileriniz başarıyla güncellendi.");

        }
        // Değişiklik varsa Kaydet butonu aktif olsun
        private bool CanSaveChanges() => IsDirty;

        [RelayCommand(CanExecute = nameof(CanChangePassword))]
        private async Task ChangePassword()
        {
             // Basit Doğrulamalar
            if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                 await _dialogCoordinator.ShowMessageAsync(this, "Eksik Bilgi", "Lütfen tüm şifre alanlarını doldurun.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 return;
            }
             if (NewPassword.Length < 6) // Örnek kural
            {
                 await _dialogCoordinator.ShowMessageAsync(this, "Geçersiz Şifre", "Yeni şifre en az 6 karakter olmalıdır.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 return;
            }
             if (NewPassword != ConfirmNewPassword)
             {
                 await _dialogCoordinator.ShowMessageAsync(this, "Şifreler Uyuşmuyor", "Yeni şifre ve tekrarı aynı değil.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 return;
             }

             // --- MEVCUT ŞİFRE DOĞRULAMA VE YENİ ŞİFRE KAYDETME (Simülasyon) ---
              // Gerçek uygulamada mevcut şifre servise gönderilip doğrulanır.
              // Örneğin: bool isCurrentPasswordValid = await AuthService.VerifyPasswordAsync(Username, CurrentPassword);
              bool isCurrentPasswordValid = CurrentPassword == "123456"; // Simülasyon: Mevcut şifre "123456" olsun

             if (!isCurrentPasswordValid)
             {
                  await _dialogCoordinator.ShowMessageAsync(this, "Hatalı Şifre", "Girdiğiniz mevcut şifre yanlış.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                  return;
             }

             // Şifre değiştirme onayı
             var result = await _dialogCoordinator.ShowMessageAsync(this, "Şifre Değiştirme Onayı", "Şifrenizi değiştirmek istediğinize emin misiniz? Bu işlemden sonra yeni şifrenizle giriş yapmanız gerekecektir.",
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Evet, Değiştir", NegativeButtonText="İptal"});

             if (result != MessageDialogResult.Affirmative) return;


             var progress = await _dialogCoordinator.ShowProgressAsync(this, "İşleniyor...", "Şifreniz güncelleniyor...");
             progress.SetIndeterminate();
             await Task.Delay(1500); // Simülasyon

             // Gerçek şifre değiştirme işlemi (servis çağrısı)
             // bool success = await AuthService.ChangePasswordAsync(Username, NewPassword);
              bool success = true; // Simülasyon

             await progress.CloseAsync();

             if(success)
             {
                await _dialogCoordinator.ShowMessageAsync(this, "Başarılı", "Şifreniz başarıyla değiştirildi.");
                // Alanları temizle ve paneli gizle
                IsPasswordChangeVisible = false;
                CurrentPassword = ""; NewPassword = ""; ConfirmNewPassword = "";
             }
             else
             {
                  await _dialogCoordinator.ShowMessageAsync(this, "Hata", "Şifre değiştirilirken bir sorun oluştu. Lütfen tekrar deneyin.");
             }
             // ------------------------------------------
        }
         // Şifre Değiştir butonu, alanlar boş değilse aktif olsun
        private bool CanChangePassword() =>
            !string.IsNullOrWhiteSpace(CurrentPassword) &&
            !string.IsNullOrWhiteSpace(NewPassword) &&
            !string.IsNullOrWhiteSpace(ConfirmNewPassword);

        // Özellik Değişikliklerini İzleme
        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (!_isInitializing)
            {
                // İlgili alanlar değiştiğinde IsDirty'yi kontrol et/ayarla
                 if (e.PropertyName == nameof(FullName) ||
                    e.PropertyName == nameof(Email) ||
                    e.PropertyName == nameof(PhoneNumber))
                {
                     IsDirty = FullName != _originalFullName ||
                               Email != _originalEmail ||
                               PhoneNumber != _originalPhoneNumber;
                }
                 // Şifre alanları değiştiğinde ChangePasswordCommand'ın durumunu güncelle
                 else if (e.PropertyName == nameof(CurrentPassword) ||
                          e.PropertyName == nameof(NewPassword) ||
                          e.PropertyName == nameof(ConfirmNewPassword))
                 {
                     ChangePasswordCommand.NotifyCanExecuteChanged();
                 }

            }
        }
    }
}