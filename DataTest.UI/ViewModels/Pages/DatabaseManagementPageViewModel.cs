using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace DataTest.UI.ViewModels.Pages;

    public partial class DatabaseManagementPageViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        [ObservableProperty]
        private string _databaseType = "SQL Server (Örnek)"; // Veritabanı türü

        [ObservableProperty]
        private string _serverName = "(localdb)\\MSSQLLocalDB"; // Sunucu adı

        [ObservableProperty]
        private string _databaseName = "OtelYonetimDB"; // Veritabanı adı

        [ObservableProperty]
        private string _connectionStatus = "Bilinmiyor"; // Bağlantı durumu

        [ObservableProperty]
        private DateTime? _lastBackupDate;

        [ObservableProperty]
        private string _lastBackupStatus = "Yok";

        [ObservableProperty]
        private string _backupLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OtelYedekleri"); // Varsayılan yedekleme konumu

        [ObservableProperty]
        private string _restoreFilePath;

        [ObservableProperty]
        private DateTime? _purgeCutoffDate = DateTime.Today.AddYears(-2); // 2 yıldan eski veriler (örnek)

        [ObservableProperty]
        private bool _isBusy; // İşlem sırasında UI'ı kilitlemek için

        [ObservableProperty]
        private ObservableCollection<string> _operationLog; // İşlem günlüğü

        public DatabaseManagementPageViewModel(IDialogCoordinator dialogCoordinator = null)
        {
            _dialogCoordinator = dialogCoordinator ?? DialogCoordinator.Instance;
            OperationLog = new ObservableCollection<string>();
            LogOperation("Veritabanı Yönetimi sayfası yüklendi.");
            // Başlangıçta durumu kontrol et
            TestConnectionCommand.Execute(null);
            // Varsayılan yedekleme klasörünü kontrol et/oluştur
            EnsureBackupDirectoryExists();
        }
         public DatabaseManagementPageViewModel() : this(DialogCoordinator.Instance) { }


        private void LogOperation(string message)
        {
            // Zaman damgası ile log ekle (en üste)
            OperationLog.Insert(0, $"{DateTime.Now:G}: {message}");
            // Listeyi çok uzatmamak için eski logları sil (opsiyonel)
            if (OperationLog.Count > 100)
            {
                OperationLog.RemoveAt(OperationLog.Count - 1);
            }
        }
         private void EnsureBackupDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(BackupLocation))
                {
                    Directory.CreateDirectory(BackupLocation);
                    LogOperation($"Varsayılan yedekleme klasörü oluşturuldu: {BackupLocation}");
                }
            }
            catch (Exception ex)
            {
                 LogOperation($"Yedekleme klasörü oluşturulamadı: {ex.Message}");
                 // Hata mesajı gösterilebilir
            }
        }


        [RelayCommand(CanExecute = nameof(CanExecuteDefaultCommand))]
        private async Task TestConnection()
        {
            LogOperation("Bağlantı testi başlatıldı...");
            IsBusy = true;
            ConnectionStatus = "Test ediliyor...";
            await Task.Delay(1500); // Gerçek bağlantı testini simüle et

            // --- Gerçek Bağlantı Testi Kodu Burada Olmalı ---
            bool success = new Random().Next(0, 10) > 1; // %90 başarı olasılığı (simülasyon)
            // ------------------------------------------------

            if (success)
            {
                ConnectionStatus = "Bağlı";
                LogOperation("Veritabanı bağlantısı başarılı.");
            }
            else
            {
                ConnectionStatus = "Bağlantı Hatası!";
                LogOperation("HATA: Veritabanına bağlanılamadı.");
                 await _dialogCoordinator.ShowMessageAsync(this, "Bağlantı Hatası", "Veritabanı sunucusuna bağlanılamadı. Ayarları kontrol edin.", MessageDialogStyle.Affirmative, new MetroDialogSettings {AffirmativeButtonText="Tamam"});
            }
            IsBusy = false;
        }

        [RelayCommand(CanExecute = nameof(CanExecuteDefaultCommand))]
        private void SelectBackupLocation()
        {
             // Ookii.Dialogs.Wpf NuGet paketini kullanıyoruz
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Veritabanı yedeklerinin kaydedileceği klasörü seçin:",
                UseDescriptionForTitle = true,
                SelectedPath = Directory.Exists(BackupLocation) ? BackupLocation : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) // Başlangıç konumu
            };

            if (dialog.ShowDialog() == true)
            {
                BackupLocation = dialog.SelectedPath;
                LogOperation($"Yedekleme konumu değiştirildi: {BackupLocation}");
                 EnsureBackupDirectoryExists(); // Yeni konumu da kontrol et/oluştur
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteDefaultCommand))]
        private async Task BackupDatabase()
        {
             var confirmSettings = new MetroDialogSettings { AffirmativeButtonText = "Evet, Yedekle", NegativeButtonText = "İptal", AnimateShow=true };
             var result = await _dialogCoordinator.ShowMessageAsync(this, "Yedekleme Onayı",
                $"Veritabanı '{DatabaseName}' şimdi yedeklenecek.\nYedekleme konumu: {BackupLocation}\n\nDevam etmek istiyor musunuz?",
                MessageDialogStyle.AffirmativeAndNegative, confirmSettings);

             if (result != MessageDialogResult.Affirmative)
             {
                 LogOperation("Yedekleme işlemi iptal edildi.");
                 return;
             }

            LogOperation("Veritabanı yedekleme işlemi başlatıldı...");
            IsBusy = true;
            var progressDialog = await _dialogCoordinator.ShowProgressAsync(this, "Yedekleniyor...", "Lütfen bekleyin, veritabanı yedekleniyor.", isCancelable: false);
            progressDialog.SetIndeterminate(); // Sürekli dönen progress bar

            string backupFileName = $"{DatabaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string fullBackupPath = Path.Combine(BackupLocation, backupFileName);
            bool success = false;

            try
            {
                // --- Gerçek Yedekleme Kodu Burada Olmalı ---
                // Örnek: System.Data.SqlClient veya EF Core ile yedekleme komutu çalıştırılır.
                // SqlCommand cmd = new SqlCommand($"BACKUP DATABASE [{DatabaseName}] TO DISK = N'{fullBackupPath}' WITH NOFORMAT, NOINIT, NAME = N'{DatabaseName}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", connection);
                // cmd.ExecuteNonQuery();
                await Task.Delay(5000); // Yedekleme işlemini simüle et (5 saniye)
                success = true; // Simülasyon başarılı
                // ------------------------------------------
            }
            catch (Exception ex)
            {
                 LogOperation($"HATA: Yedekleme sırasında bir hata oluştu: {ex.Message}");
                 await progressDialog.CloseAsync();
                 IsBusy = false;
                 await _dialogCoordinator.ShowMessageAsync(this, "Yedekleme Hatası", $"Veritabanı yedeklenirken bir hata oluştu:\n{ex.Message}", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 return;
            }


            await progressDialog.CloseAsync();
            IsBusy = false;

            if (success)
            {
                LastBackupDate = DateTime.Now;
                LastBackupStatus = "Başarılı";
                LogOperation($"Veritabanı başarıyla yedeklendi: {fullBackupPath}");
                await _dialogCoordinator.ShowMessageAsync(this, "Yedekleme Başarılı", $"Veritabanı başarıyla '{backupFileName}' dosyasına yedeklendi.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
            }
            else
            {
                 LastBackupStatus = "Başarısız!";
                 LogOperation("HATA: Yedekleme işlemi başarısız oldu (simülasyon).");
                 // Hata mesajı zaten catch bloğunda gösterildi.
            }

        }


        [RelayCommand(CanExecute = nameof(CanExecuteDefaultCommand))]
        private void SelectRestoreFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Veritabanı Yedek Dosyaları (*.bak)|*.bak|Tüm Dosyalar (*.*)|*.*",
                Title = "Geri Yüklenecek Yedek Dosyasını Seçin",
                InitialDirectory = Directory.Exists(BackupLocation) ? BackupLocation : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                RestoreFilePath = dialog.FileName;
                LogOperation($"Geri yüklenecek dosya seçildi: {RestoreFilePath}");
            }
             else
             {
                 LogOperation("Dosya seçimi iptal edildi.");
                 RestoreFilePath = null; // Seçimi temizle
             }
        }


        [RelayCommand(CanExecute = nameof(CanExecuteRestoreCommand))]
        private async Task RestoreDatabase()
        {
             var warningSettings = new MetroDialogSettings { AffirmativeButtonText = "EVET, EMİNİM, GERİ YÜKLE!", NegativeButtonText = "Hayır, İptal Et", AnimateShow=true};
             var result = await _dialogCoordinator.ShowMessageAsync(this, "!!! ÇOK ÖNEMLİ UYARI !!!",
                $"'{Path.GetFileName(RestoreFilePath)}' dosyasından geri yükleme yapacaksınız.\n\n" +
                "BU İŞLEM MEVCUT VERİTABANINDAKİ TÜM VERİLERİ SİLECEK VE YEDEKTEKİ VERİLERLE DEĞİŞTİRECEKTİR!\n\n" +
                "Bu işlem geri alınamaz. Emin misiniz?",
                MessageDialogStyle.AffirmativeAndNegative, warningSettings);

             if (result != MessageDialogResult.Affirmative)
             {
                 LogOperation("Geri yükleme işlemi kullanıcı tarafından iptal edildi.");
                 return;
             }

            // İkinci bir onay isteyelim (daha da emin olmak için)
            string confirmationText = "GERIYUKLE";
            var inputResult = await _dialogCoordinator.ShowInputAsync(this, "Son Onay", $"Geri yükleme işlemine devam etmek için lütfen '{confirmationText}' yazın:");

            if (inputResult?.Trim().ToUpper() != confirmationText)
            {
                 LogOperation("Geri yükleme işlemi son onayda iptal edildi veya yanlış metin girildi.");
                 await _dialogCoordinator.ShowMessageAsync(this,"İptal Edildi", "Geri yükleme işlemi iptal edildi.");
                 return;
            }


            LogOperation($"Veritabanı geri yükleme işlemi başlatıldı: {RestoreFilePath}");
            IsBusy = true;
            var progressDialog = await _dialogCoordinator.ShowProgressAsync(this, "Geri Yükleniyor...", "Lütfen bekleyin, veritabanı yedekten geri yükleniyor.", isCancelable: false);
            progressDialog.SetIndeterminate();
            bool success = false;

             try
            {
                 // --- Gerçek Geri Yükleme Kodu Burada Olmalı ---
                 // ÖNEMLİ: Geri yükleme genellikle veritabanına özel erişim gerektirir (tek kullanıcı modu vb.)
                 // ve çalışan uygulamanın durdurulması gerekebilir.
                 // Örnek: SQL Server için ALTER DATABASE .. SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE ...; ALTER DATABASE .. SET MULTI_USER;
                 await Task.Delay(8000); // Geri yükleme işlemini simüle et (8 saniye)
                 success = true; // Simülasyon başarılı
                 // ------------------------------------------
            }
            catch (Exception ex)
            {
                 LogOperation($"HATA: Geri yükleme sırasında bir hata oluştu: {ex.Message}");
                 await progressDialog.CloseAsync();
                 IsBusy = false;
                 await _dialogCoordinator.ShowMessageAsync(this, "Geri Yükleme Hatası", $"Veritabanı geri yüklenirken bir hata oluştu:\n{ex.Message}", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 return;
            }

            await progressDialog.CloseAsync();
            IsBusy = false;

            if (success)
            {
                 LogOperation($"Veritabanı başarıyla geri yüklendi: {RestoreFilePath}");
                 await _dialogCoordinator.ShowMessageAsync(this, "Geri Yükleme Başarılı", "Veritabanı başarıyla seçilen yedekten geri yüklendi.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 // Geri yükleme sonrası belki bağlantı tekrar test edilmeli
                 TestConnectionCommand.Execute(null);
            }
             else
             {
                 LogOperation("HATA: Geri yükleme işlemi başarısız oldu (simülasyon).");
             }
        }
        // Geri yükleme butonu sadece dosya seçiliyse aktif olsun
        private bool CanExecuteRestoreCommand() => !IsBusy && !string.IsNullOrEmpty(RestoreFilePath) && File.Exists(RestoreFilePath);


        [RelayCommand(CanExecute = nameof(CanExecuteDefaultCommand))]
        private async Task OptimizeDatabase()
        {
             LogOperation("Veritabanı optimizasyon işlemi başlatıldı...");
             IsBusy = true;
              var progressDialog = await _dialogCoordinator.ShowProgressAsync(this, "Optimize Ediliyor...", "Veritabanı bakım ve optimizasyon işlemleri yapılıyor...", isCancelable: false);
             progressDialog.SetIndeterminate();

             // --- Gerçek Optimizasyon Kodu Burada Olmalı ---
             // Örnek: Indexleri yeniden oluşturma, istatistikleri güncelleme vb.
             // DBCC REINDEX, sp_updatestats vb. komutlar (SQL Server için)
             await Task.Delay(4000); // Simülasyon
             // ------------------------------------------

              await progressDialog.CloseAsync();
             LogOperation("Veritabanı optimizasyon işlemi tamamlandı.");
             IsBusy = false;
             await _dialogCoordinator.ShowMessageAsync(this, "Optimizasyon Tamamlandı", "Veritabanı optimizasyon ve bakım işlemleri başarıyla tamamlandı.", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
        }


        [RelayCommand(CanExecute = nameof(CanExecutePurgeCommand))]
        private async Task PurgeOldData()
        {
            var cutoffDate = PurgeCutoffDate ?? DateTime.MinValue; // Null ise çok eski bir tarih al
            var warningSettings = new MetroDialogSettings { AffirmativeButtonText = "Evet, Eski Verileri Sil", NegativeButtonText = "İptal Et", AnimateShow=true };
            var result = await _dialogCoordinator.ShowMessageAsync(this, "!!! Veri Silme Uyarısı !!!",
               $"'{cutoffDate:d MMMM yyyy}' tarihinden önceki tüm kayıtları (rezervasyonlar, ödemeler vb.) kalıcı olarak silmek üzeresiniz.\n\n" +
               "BU İŞLEM GERİ ALINAMAZ!\n\n" +
               "Silme işlemine devam etmek istediğinize emin misiniz?",
               MessageDialogStyle.AffirmativeAndNegative, warningSettings);

             if (result != MessageDialogResult.Affirmative)
             {
                 LogOperation("Eski veri temizleme işlemi iptal edildi.");
                 return;
             }

             LogOperation($"Eski veri temizleme işlemi başlatıldı (Tarih < {cutoffDate:d MMMM yyyy})...");
             IsBusy = true;
             var progressDialog = await _dialogCoordinator.ShowProgressAsync(this, "Veriler Siliniyor...", $"'{cutoffDate:d MMMM yyyy}' tarihinden eski veriler siliniyor...", isCancelable: false);
             progressDialog.SetIndeterminate();
             int deletedRecordCount = 0; // Silinen kayıt sayısı (simülasyon)

            try
            {
                 // --- Gerçek Veri Silme Kodu Burada Olmalı ---
                 // Örnek: DELETE FROM Reservations WHERE CheckOutDate < @CutoffDate;
                 //        DELETE FROM Payments WHERE PaymentDate < @CutoffDate; vb.
                 // Bu işlemler transaction içinde yapılmalı.
                  await Task.Delay(6000); // Simülasyon
                  deletedRecordCount = new Random().Next(50, 500); // Rastgele silinen kayıt sayısı
                 // ------------------------------------------
            }
            catch (Exception ex)
            {
                 LogOperation($"HATA: Eski veriler silinirken bir hata oluştu: {ex.Message}");
                 await progressDialog.CloseAsync();
                 IsBusy = false;
                 await _dialogCoordinator.ShowMessageAsync(this, "Silme Hatası", $"Eski veriler silinirken bir hata oluştu:\n{ex.Message}", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});
                 return;
            }

            await progressDialog.CloseAsync();
            LogOperation($"Eski veri temizleme işlemi tamamlandı. {deletedRecordCount} kayıt silindi (simülasyon).");
            IsBusy = false;
            await _dialogCoordinator.ShowMessageAsync(this, "Temizlik Tamamlandı", $"'{cutoffDate:d MMMM yyyy}' tarihinden eski veriler başarıyla silindi. Silinen kayıt sayısı: {deletedRecordCount} (simülasyon).", MessageDialogStyle.Affirmative, new MetroDialogSettings{AffirmativeButtonText="Tamam"});

        }
        // Temizleme butonu sadece tarih seçiliyse aktif olsun
         private bool CanExecutePurgeCommand() => !IsBusy && PurgeCutoffDate.HasValue;


         // İşlem yokken çalışabilecek komutlar için genel CanExecute
         private bool CanExecuteDefaultCommand() => !IsBusy;

         // RestoreFilePath değiştiğinde CanExecuteRestoreCommand'ın durumunu güncelle
         partial void OnRestoreFilePathChanged(string value)
         {
             RestoreDatabaseCommand.NotifyCanExecuteChanged();
         }
         // PurgeCutoffDate değiştiğinde CanExecutePurgeCommand'ın durumunu güncelle
         partial void OnPurgeCutoffDateChanged(DateTime? value)
         {
            PurgeOldDataCommand.NotifyCanExecuteChanged();
         }
         // IsBusy değiştiğinde tüm komutların durumunu güncelle
         partial void OnIsBusyChanged(bool value)
         {
             TestConnectionCommand.NotifyCanExecuteChanged();
             SelectBackupLocationCommand.NotifyCanExecuteChanged();
             BackupDatabaseCommand.NotifyCanExecuteChanged();
             SelectRestoreFileCommand.NotifyCanExecuteChanged();
             RestoreDatabaseCommand.NotifyCanExecuteChanged();
             OptimizeDatabaseCommand.NotifyCanExecuteChanged();
             PurgeOldDataCommand.NotifyCanExecuteChanged();
         }
    }