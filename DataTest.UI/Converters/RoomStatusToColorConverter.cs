using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DataTest.UI.Converters
{
    public class RoomStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "müsait" => new SolidColorBrush(Color.FromRgb(198, 239, 206)),     // Açık Yeşil
                    "dolu" => new SolidColorBrush(Color.FromRgb(255, 199, 206)),       // Açık Kırmızı
                    "temizleniyor" => new SolidColorBrush(Color.FromRgb(189, 215, 238)), // Açık Mavi
                    "bakımda" => new SolidColorBrush(Color.FromRgb(255, 235, 156)),    // Açık Turuncu
                    _ => new SolidColorBrush(Colors.LightGray)
                };
            }

            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 