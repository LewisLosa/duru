using System.Windows.Controls;
using DataTest.UI.ViewModels.Pages;
using LiveCharts; // ChartPoint için
using LiveCharts.Wpf; // PieChart için
using System.Windows; // MessageBox için

namespace DataTest.UI.Pages
{
    /// <summary>
    /// ReportingAndAnalysisPage.xaml etkileşim mantığı
    /// </summary>
    public partial class ReportingAndAnalysisPage : Page
    {
        public ReportingAndAnalysisPage()
        {
            InitializeComponent();
            DataContext = new ReportingViewModel();
        }

        // İsteğe Bağlı: Pasta grafiği dilimine tıklandığında bilgi gösterme
        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;

            // Tıklanan dilimi seçili (patlamış) hale getir
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8; // Seçili dilimi biraz dışarı çıkar

            // Detay göstermek için MessageBox (veya başka bir işlem)
            var seriesTitle = selectedSeries.Title ?? "Bilinmeyen";
            MessageBox.Show($"Seçilen Kategori: {seriesTitle}\nDeğer: {chartPoint.Y:N0}\nPay: {chartPoint.Participation:P}",
                "Pasta Dilimi Detayı", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}