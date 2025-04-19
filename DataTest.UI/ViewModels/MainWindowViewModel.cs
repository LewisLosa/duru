using DataTest.UI.Pages;
using DataTest.UI.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System.Collections.ObjectModel;
using System.Windows;
namespace DataTest.UI.ViewModels;
public class MainWindowViewModel : BindableBase
{
    public ObservableCollection<object> Menu { get; } = new();

    public ObservableCollection<MenuItem> OptionsMenu { get; } = new();

    public MainWindowViewModel()
    {
        // Build the menus
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.HouseSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Ana Sayfa",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/HomePage.xaml", UriKind.RelativeOrAbsolute)
        });

        this.Menu.Add(new HamburgerMenuSeparatorItem());

        // Rezervasyon Yönetimi
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CalendarSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Rezervasyon Yönetimi",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/ReservationManagementPage.xaml", UriKind.RelativeOrAbsolute)
        });

        // Oda Yönetimi
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.BedSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Oda Yönetimi",
            NavigationType = typeof(RoomManagementPage),
            NavigationDestination = new Uri("Pages/RoomManagementPage.xaml", UriKind.RelativeOrAbsolute)
        });

        // Misafir Yönetimi
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.UserSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Misafir Yönetimi",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/GuestManagementPage.xaml", UriKind.RelativeOrAbsolute)
        });

        // Ödeme Yönetimi
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CreditCardSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Ödeme Yönetimi",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/PaymentManagementPage.xaml", UriKind.RelativeOrAbsolute)
        });

        this.Menu.Add(new HamburgerMenuSeparatorItem());

        // Raporlama ve Analiz
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ChartBarSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Raporlama ve Analiz",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/ReportingAndAnalysisPage.xaml", UriKind.RelativeOrAbsolute)
        });

        // Entegrasyonlar
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.LinkSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Entegrasyonlar",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/IntegrationsPage.xaml", UriKind.RelativeOrAbsolute)
        });

        // Veritabanı Yönetimi
        this.Menu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.DatabaseSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Veritabanı Yönetimi",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/DatabaseManagementPage.xaml", UriKind.RelativeOrAbsolute)
        });

        this.OptionsMenu.Add(new MenuItem()
        {
            Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.UserSolid, Style = Application.Current.Resources["HamburgerIconStyle"] as Style },
            Label = "Eyüp Şengöz",
            Tag = "Developer",
            NavigationType = typeof(HomePage),
            NavigationDestination = new Uri("Pages/UserProfilePage.xaml", UriKind.RelativeOrAbsolute)
        });
    }
}