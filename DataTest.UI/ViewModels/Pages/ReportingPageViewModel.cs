using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts; // LiveCharts çekirdek sınıfları
using LiveCharts.Wpf; // LiveCharts WPF özel sınıfları
using System.Windows.Media; // Renkler için

namespace DataTest.UI.ViewModels.Pages
{
    public partial class ReportingViewModel : ObservableObject
    {
        // --- KPI Özellikleri ---
        [ObservableProperty]
        private decimal _totalRevenue;

        [ObservableProperty]
        private double _occupancyRate;

        [ObservableProperty]
        private decimal _averageDailyRate;

        [ObservableProperty]
        private decimal _revPAR; // Revenue Per Available Room

        // --- Filtreleme Özellikleri ---
        [ObservableProperty]
        private DateTime _reportStartDate = DateTime.Today.AddMonths(-1);

        [ObservableProperty]
        private DateTime _reportEndDate = DateTime.Today;

        // --- Grafik Verileri ---

        // Gelir Grafiği (Çizgi)
        [ObservableProperty]
        private SeriesCollection _revenueSeries;

        [ObservableProperty]
        private string[] _revenueLabels; // X Ekseni Etiketleri (Tarihler)
        public Func<double, string> RevenueYFormatter { get; set; } // Y Ekseni Formatlayıcı (Para Birimi)

        // Doluluk Oranı Grafiği (Sütun)
        [ObservableProperty]
        private SeriesCollection _occupancySeries;
         [ObservableProperty]
        private string[] _occupancyLabels; // X Ekseni Etiketleri (Tarihler)
        public Func<double, string> OccupancyYFormatter { get; set; } // Y Ekseni Formatlayıcı (Yüzde)


        // Oda Tipi Gelir Dağılımı (Pasta)
        [ObservableProperty]
        private SeriesCollection _roomTypeRevenueSeries;
        public Func<ChartPoint, string> RoomTypePointLabel { get; set; } // Pasta dilimi etiketi formatı


        public ReportingViewModel()
        {
            // Formatlayıcıları ayarla
            RevenueYFormatter = value => value.ToString("C0"); // Para birimi, sıfır ondalık
            OccupancyYFormatter = value => value.ToString("P0"); // Yüzde, sıfır ondalık
            RoomTypePointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation); // Değer (Yüzde)

            LoadReportData(); // Başlangıç verilerini yükle
        }

        [RelayCommand]
        private void LoadReportData()
        {
             // Gerçek uygulamada bu veriler seçilen tarih aralığına göre
             // veritabanından veya servislerden sorgulanır.
             // Şimdilik rastgele örnek veriler oluşturalım.

             GenerateSampleRevenueData();
             GenerateSampleOccupancyData();
             GenerateSampleRoomTypeRevenueData();
             CalculateSampleKPIs();
        }

        private void GenerateSampleRevenueData()
        {
            var startDate = ReportStartDate;
            var endDate = ReportEndDate;
            var dayCount = (int)(endDate - startDate).TotalDays + 1;
            if (dayCount <= 0) dayCount = 1;

            var labels = new List<string>();
            var revenueValues = new ChartValues<decimal>();
            var random = new Random();

            for (int i = 0; i < dayCount; i++)
            {
                var currentDate = startDate.AddDays(i);
                labels.Add(currentDate.ToString("dd MMM")); // Gün ve Ay formatı
                revenueValues.Add(random.Next(1500, 5000)); // Rastgele günlük gelir
            }

            RevenueLabels = labels.ToArray();
            RevenueSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Günlük Gelir",
                    Values = revenueValues,
                    PointGeometry = null, // Noktaları gösterme
                    Fill = Brushes.Transparent // Alan dolgusunu kaldır
                }
            };
        }
         private void GenerateSampleOccupancyData()
        {
            var startDate = ReportStartDate;
            var endDate = ReportEndDate;
            var dayCount = (int)(endDate - startDate).TotalDays + 1;
            if (dayCount <= 0) dayCount = 1;

            var labels = new List<string>();
            var occupancyValues = new ChartValues<double>();
            var random = new Random();

            for (int i = 0; i < dayCount; i++)
            {
                var currentDate = startDate.AddDays(i);
                labels.Add(currentDate.ToString("dd MMM"));
                // Rastgele doluluk oranı (%50 - %95 arası)
                occupancyValues.Add(random.NextDouble() * (0.95 - 0.50) + 0.50);
            }

            OccupancyLabels = labels.ToArray();
            OccupancySeries = new SeriesCollection
            {
                new ColumnSeries // Sütun grafik olarak gösterelim
                {
                    Title = "Günlük Doluluk",
                    Values = occupancyValues,
                     DataLabels = false // Sütun üzerindeki etiketleri kapat
                }
            };
        }


        private void GenerateSampleRoomTypeRevenueData()
        {
            // Örnek oda tipi gelirleri
            RoomTypeRevenueSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Tek Kişilik",
                    Values = new ChartValues<double> { 45000 }, // Toplam gelir
                    DataLabels = true, // Dilim üzerinde etiket göster
                    LabelPoint = RoomTypePointLabel // Özel etiket formatı
                },
                new PieSeries
                {
                    Title = "Çift Kişilik",
                    Values = new ChartValues<double> { 72000 },
                    DataLabels = true,
                    LabelPoint = RoomTypePointLabel
                },
                new PieSeries
                {
                    Title = "Suit",
                    Values = new ChartValues<double> { 35000 },
                    DataLabels = true,
                    LabelPoint = RoomTypePointLabel
                },
                 new PieSeries
                {
                    Title = "Aile Odası",
                    Values = new ChartValues<double> { 18000 },
                    DataLabels = true,
                    LabelPoint = RoomTypePointLabel
                }
            };
        }

        private void CalculateSampleKPIs()
        {
             // Örnek KPI hesaplamaları (gerçek veriye göre yapılmalı)
             var random = new Random();
             TotalRevenue = (RevenueSeries?.FirstOrDefault()?.Values as ChartValues<decimal>)?.Sum() ?? 0m; // Gelir grafiğindeki toplam
             OccupancyRate = (OccupancySeries?.FirstOrDefault()?.Values as ChartValues<double>)?.Average() ?? 0.0; // Ortalama doluluk
             AverageDailyRate = TotalRevenue > 0 && OccupancyRate > 0 ? TotalRevenue / (decimal)(30 * OccupancyRate) / 50m : random.Next(250, 500); // Basit örnek hesaplama
             RevPAR = AverageDailyRate * (decimal)OccupancyRate; // ADR * Doluluk Oranı
        }
    }
}