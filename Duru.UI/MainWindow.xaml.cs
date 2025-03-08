using System.Windows;
using System.Windows.Controls;
using Common.Library;
using Duru.UI.Data.Entities;
using Duru.UI.ViewModel;

namespace Duru.UI
{
    /// <summary>
    /// Main window of the application handling core UI functionality and navigation
    /// Implements IDisposable for proper resource cleanup
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        // Path constant for the login control to prevent magic strings
        private const string? LoginControlPath = "Duru.UI.Pages.LoginControl";
        
        #region Private Variables
        // View model instance for data binding and UI logic
        private readonly MainWindowViewModel _viewModel;
        // Stores the initial status message for restoration
        private readonly string? _originalMessage;
        // Flag to prevent multiple dispose calls
        private bool _disposed;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the main window, sets up view model binding and message broker subscription
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = Resources["ViewModel"] as MainWindowViewModel 
                ?? throw new InvalidOperationException("ViewModel not found in resources");

            if (_viewModel.StatusMessage != null) _originalMessage = _viewModel.StatusMessage;
            MessageBroker.Instance.MessageReceived += Instance_MessageReceived;
        }
        #endregion

        #region Window Events
        /// <summary>
        /// Handles initial application loading when window is loaded
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadApplication();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load application: " + ex.Message, "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Message Broker Events
        /// <summary>
        /// Handles all incoming messages from MessageBroker and ensures UI thread synchronization
        /// </summary>
        private async void Instance_MessageReceived(object? sender, MessageBrokerEventArgs? e)
        {
            if (e == null) return;

            await Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    ProcessMessage(e);
                }
                catch (Exception ex)
                {
                    _viewModel.StatusMessage = "Error processing message: " + ex.Message;
                }
            });
        }

        /// <summary>
        /// Processes different types of messages and updates UI accordingly
        /// </summary>
        private void ProcessMessage(MessageBrokerEventArgs? e)
        {
            switch (e?.MessageName)
            {
                case MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE:
                    _viewModel.InfoMessageTitle = e.MessagePayload.ToString();
                    _viewModel.CreateInfoMessageTimer();
                    break;

                case MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE:
                    _viewModel.InfoMessage = e.MessagePayload.ToString();
                    _viewModel.CreateInfoMessageTimer();
                    break;

                case MessageBrokerMessages.DISPLAY_STATUS_MESSAGE:
                    _viewModel.StatusMessage = e.MessagePayload.ToString();
                    break;

                case MessageBrokerMessages.LOGIN_SUCCESS:
                    if (e.MessagePayload is Employee employee)
                    {
                        _viewModel.EmployeeEntity = employee;
                        _viewModel.LoginMenuHeader = $"Logout {employee.FirstName} {employee.LastName}";
                    }
                    break;

                case MessageBrokerMessages.LOGOUT:
                    _viewModel.EmployeeEntity.IsLoggedIn = false;
                    _viewModel.LoginMenuHeader = "Login";
                    break;

                case MessageBrokerMessages.CLOSE_USER_CONTROL:
                    CloseUserControl();
                    break;
            }
        }
        #endregion

        #region Menu Handling
        /// <summary>
        /// Handles menu item clicks and routes to appropriate handlers
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem || menuItem.Tag == null) return;

            string? command = menuItem.Tag.ToString();
            if (string.IsNullOrEmpty(command)) return;

            if (command.Contains('.'))
            {
                LoadUserControl(command);
            }
            else
            {
                ProcessMenuCommands(command);
            }
        }

        /// <summary>
        /// Processes special menu commands like exit and login
        /// </summary>
        private void ProcessMenuCommands(string? command)
        {
            if (string.IsNullOrEmpty(command)) return;

            switch (command.ToLower())
            {
                case "exit":
                    Close();
                    break;

                case "login":
                    HandleLoginCommand();
                    break;
            }
        }

        /// <summary>
        /// Handles login/logout functionality
        /// </summary>
        private void HandleLoginCommand()
        {
            if (_viewModel.EmployeeEntity.IsLoggedIn)
            {
                CloseUserControl();
                _viewModel.EmployeeEntity = new Employee();
                _viewModel.LoginMenuHeader = "Login";
            }
            else
            {
                LoadUserControl(LoginControlPath);
            }
        }
        #endregion

        #region User Control Management
        /// <summary>
        /// Loads a user control dynamically by its type name
        /// </summary>
        private void LoadUserControl(string? controlName)
        {
            if (string.IsNullOrEmpty(controlName) || !ShouldLoadUserControl(controlName))
                return;

            try
            {
                Type ucType = Type.GetType(controlName) 
                    ?? throw new InvalidOperationException($"Control type {controlName} not found");

                var uc = Activator.CreateInstance(ucType) as UserControl 
                    ?? throw new InvalidOperationException($"Failed to create control of type {controlName}");

                CloseUserControl();
                DisplayUserControl(uc);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading control {controlName}: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Displays a user control in the content area
        /// </summary>
        public void DisplayUserControl(UserControl uc)
        {
            if (uc == null) throw new ArgumentNullException(nameof(uc));
            ContentArea.Children.Add(uc);
        }

        /// <summary>
        /// Checks if a user control should be loaded based on current state
        /// </summary>
        private bool ShouldLoadUserControl(string? controlName)
        {
            if (string.IsNullOrEmpty(controlName)) return false;
            
            return ContentArea.Children.Count == 0 || 
                   ((UserControl)ContentArea.Children[0]).GetType().FullName != controlName;
        }

        /// <summary>
        /// Closes current user control and restores original status
        /// </summary>
        private void CloseUserControl()
        {
            ContentArea.Children.Clear();
            _viewModel.StatusMessage = _originalMessage;
        }
        #endregion

        #region Application Loading
        /// <summary>
        /// Initializes application data and handles loading states
        /// </summary>
        public async Task LoadApplication()
        {
            try
            {
                _viewModel.IsInfoMessageVisible = true;
                await LoadAllDataAsync();
            }
            catch (Exception)
            {
                _viewModel.StatusMessage = "Failed to load application data";
                throw;
            }
            finally
            {
                _viewModel.IsInfoMessageVisible = false;
            }
        }

        /// <summary>
        /// Loads all required application data asynchronously
        /// </summary>
        private async Task LoadAllDataAsync()
        {
            await _viewModel.LoadStateCodesAsync();
            await _viewModel.LoadCountryCodesAsync();
            await _viewModel.LoadEmployeeTypesAsync();
        }
        #endregion

        #region IDisposable Implementation
        /// <summary>
        /// Ensures proper cleanup when window is closed
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Dispose();
        }

        /// <summary>
        /// Implements IDisposable pattern for resource cleanup
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            
            MessageBroker.Instance.MessageReceived -= Instance_MessageReceived;
            _disposed = true;
        }
        #endregion
    }
}