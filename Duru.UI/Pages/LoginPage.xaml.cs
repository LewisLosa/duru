using System.Windows;
using Duru.UI.Data.Entities;
using Duru.UI.ViewModel;

namespace Duru.UI.Pages;

public partial class LoginPage
{
    // Login view model class
    private readonly LoginPageViewModel _viewModel;
    public LoginPage()
    {
        InitializeComponent();

        try
        {
            _viewModel = Resources["ViewModel"] as LoginPageViewModel
                         ?? throw new InvalidOperationException("ViewModel not found in resources");
        }
        catch (Exception e)
        {
            MessageBox.Show("An error occurred while loading the page..." + e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Close();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_viewModel == null)
            {
                MessageBox.Show("ViewModel is not initialized", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_viewModel.Entity == null)
            {
                _viewModel.Entity = new Employee(); // Create a new entity if it doesn't exist
            }

            // Add the Password manually because data binding does not work
            if (TxtPassword != null)
            {
                _viewModel.Entity.Password = TxtPassword.Password ?? string.Empty;
                _viewModel.Login();
            }
            else
            {
                MessageBox.Show("Password field not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Login error: {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}