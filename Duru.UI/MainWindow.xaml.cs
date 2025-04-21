using System.Windows;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using MenuItem = Duru.UI.ViewModels.MenuItem;
using NavigationService = Duru.UI.Utils.NavigationService;


namespace Duru.UI;

using MenuItem = MenuItem;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    private readonly NavigationService navigationService;
    public MainWindow()
    {
        InitializeComponent();
        navigationService = new NavigationService();
        navigationService.Navigated += NavigationService_OnNavigated;
        HamburgerMenuControl.Content = navigationService.Frame;

        // Navigate to the home page.
        Loaded += (sender, args) =>
            navigationService.Navigate(new Uri("Pages/HomePage.xaml", UriKind.RelativeOrAbsolute));
    }

    private void NavigationService_OnNavigated(object sender, NavigationEventArgs e)
    {
        // select the menu item
        HamburgerMenuControl.SetCurrentValue(
            HamburgerMenu.SelectedItemProperty,
            HamburgerMenuControl.Items
                .OfType<MenuItem>()
                .FirstOrDefault(x => x.NavigationDestination == e.Uri));
        HamburgerMenuControl.SetCurrentValue(
            HamburgerMenu.SelectedOptionsItemProperty,
            HamburgerMenuControl
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
        GoBackButton.SetCurrentValue(
            VisibilityProperty, navigationService.CanGoBack ? Visibility.Visible : Visibility.Collapsed);
    }


    private void HamburgerToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (HamburgerMenuControl != null) HamburgerMenuControl.IsPaneOpen = !HamburgerMenuControl.IsPaneOpen;
    }


    private void GoBack_OnClick(object sender, RoutedEventArgs e)
    {
        navigationService.GoBack();
    }

    private void OptionsButton_Click(object sender, RoutedEventArgs e)
    {
        navigationService.Navigate(new Uri("Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
    }

    private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
    {
        if (e.InvokedItem is MenuItem menuItem && menuItem.IsNavigation)
            navigationService.Navigate(menuItem.NavigationDestination);
    }
}