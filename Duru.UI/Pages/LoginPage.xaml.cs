using System.Windows;
using Duru.UI.ViewModel;

namespace Duru.UI.Pages;

public partial class LoginPage
{
    public LoginPage()
    {
        InitializeComponent();
        
        _viewModel = Resources["ViewModel"] as LoginPageViewModel ?? throw new InvalidOperationException("ViewModel not found in resources");
    }

    // Login view model class
    private readonly LoginPageViewModel _viewModel;
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Close();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Add the Password manually because data binding does not work
        _viewModel.Entity.Password = TxtPassword.Password;
        _viewModel.Login();
    }
}