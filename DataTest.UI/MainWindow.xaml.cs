using System.Windows;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using MenuItem = DataTest.UI.ViewModels.MenuItem;


namespace DataTest.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    private readonly Utils.NavigationService navigationService;
    public MainWindow()
    {
        InitializeComponent();
        this.navigationService = new Utils.NavigationService();
        this.navigationService.Navigated += this.NavigationService_OnNavigated;
        this.HamburgerMenuControl.Content = this.navigationService.Frame;

        // Navigate to the home page.
        this.Loaded += (sender, args) => this.navigationService.Navigate(new Uri("Pages/HomePage.xaml", UriKind.RelativeOrAbsolute));
    }

    private void NavigationService_OnNavigated(object sender, NavigationEventArgs e)
    {
        // select the menu item
        this.HamburgerMenuControl.SetCurrentValue(HamburgerMenu.SelectedItemProperty,
            this.HamburgerMenuControl.Items
                .OfType<MenuItem>()
                .FirstOrDefault(x => x.NavigationDestination == e.Uri));
        this.HamburgerMenuControl.SetCurrentValue(HamburgerMenu.SelectedOptionsItemProperty,
            this.HamburgerMenuControl
                .OptionsItems
                .OfType<MenuItem>()
                .FirstOrDefault(x => x.NavigationDestination == e.Uri));

        // or when using the NavigationType on menu item
        // this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
        //                                              .Items
        //                                              .OfType<MenuItem>()
        //                                              .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());
        // this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
        //                                                     .OptionsItems
        //                                                     .OfType<MenuItem>()
        //                                                     .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());

        // update back button
        this.GoBackButton.SetCurrentValue(VisibilityProperty, this.navigationService.CanGoBack ? Visibility.Visible : Visibility.Collapsed);
    }


    private void HamburgerToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (HamburgerMenuControl != null)
        {
            HamburgerMenuControl.IsPaneOpen = !HamburgerMenuControl.IsPaneOpen;
        }
    }


    private void GoBack_OnClick(object sender, RoutedEventArgs e)
    {
        this.navigationService.GoBack();
    }
    
    private void OptionsButton_Click(object sender, RoutedEventArgs e)
    {
        this.navigationService.Navigate(new Uri("Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
    }
    
    private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
    {
        if (e.InvokedItem is MenuItem menuItem && menuItem.IsNavigation)
        {
            this.navigationService.Navigate(menuItem.NavigationDestination);
        }
    }
}