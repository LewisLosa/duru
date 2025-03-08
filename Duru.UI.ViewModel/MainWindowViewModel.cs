using System.Timers;
using Duru.Library.ViewModels;
using Duru.UI.Data.Entities;
using Timer = System.Timers.Timer;

namespace Duru.UI.ViewModel;

/// <summary>
/// ViewModel for the main window handling application state, user information, and notifications
/// Inherits from ViewModelBase for property change notifications
/// </summary>
public sealed class MainWindowViewModel : ViewModelBase
{
    #region Private Variables
    // UI state tracking variables
    private string _loginMenuHeader = "Login";
    private string? _statusMessage;

    // Info message display properties
    private bool _isInfoMessageVisible = true;
    private string? _infoMessage;
    private string? _infoMessageTitle;

    // Timer for auto-hiding info messages
    private Timer? _infoMessageTimer;
    private int _infoMessageTimeout;

    // Current logged-in employee information
    private Employee _employeeEntity = new Employee();
    #endregion

    #region Public Properties
    /// <summary>
    /// Text displayed in the login menu item
    /// Updates when user logs in/out
    /// </summary>
    public string LoginMenuHeader
    {
        get { return _loginMenuHeader; }
        set {
            _loginMenuHeader = value;
            RaisePropertyChanged("LoginMenuHeader");
        }
    }

    /// <summary>
    /// Current status message displayed in the application
    /// </summary>
    public string? StatusMessage
    {
        get { return _statusMessage; }
        set {
            _statusMessage = value;
            RaisePropertyChanged("StatusMessage");
        }
    }

    /// <summary>
    /// Controls visibility of the info message panel
    /// </summary>
    public bool IsInfoMessageVisible 
    {
        get { return _isInfoMessageVisible; }
        set {
            _isInfoMessageVisible = value;
            RaisePropertyChanged("IsInfoMessageVisible");
        }
    }

    /// <summary>
    /// Content of the information message
    /// </summary>
    public string? InfoMessage
    {
        get { return _infoMessage; }
        set {
            _infoMessage = value;
            RaisePropertyChanged("InfoMessage");
        }
    }

    /// <summary>
    /// Title of the information message
    /// </summary>
    public string? InfoMessageTitle
    {
        get { return _infoMessageTitle; }
        set {
            _infoMessageTitle = value;
            RaisePropertyChanged("InfoMessageTitle");
        }
    }

    /// <summary>
    /// Duration for which info messages are displayed
    /// </summary>
    public int InfoMessageTimeout
    {
        get { return _infoMessageTimeout; }
        set {
            _infoMessageTimeout = value;
            RaisePropertyChanged("InfoMessageTimeout");
        }
    }
    
    /// <summary>
    /// Current employee entity for the logged-in user
    /// </summary>
    public Employee EmployeeEntity
    {
        get { return _employeeEntity; }
        set {
            _employeeEntity = value;
            RaisePropertyChanged("EmployeeEntity");
        }
    }
    #endregion

    #region Message Management
    /// <summary>
    /// Clears all information messages and hides the message panel
    /// </summary>
    public void ClearInfoMessages()
    {
        InfoMessage = string.Empty;
        InfoMessageTitle = string.Empty;
        IsInfoMessageVisible = false;
    }

    /// <summary>
    /// Creates or resets the timer for auto-hiding information messages
    /// </summary>
    public void CreateInfoMessageTimer()
    {
        if (_infoMessageTimer == null)
        {
            _infoMessageTimer = new Timer(_infoMessageTimeout);
            _infoMessageTimer.Elapsed += _MessageTimer_Elapsed;
        }
        _infoMessageTimer.AutoReset = false;
        _infoMessageTimer.Enabled = true;
        IsInfoMessageVisible = true;
    }

    /// <summary>
    /// Handles the timer elapsed event to hide the info message
    /// </summary>
    private void _MessageTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        IsInfoMessageVisible = false;
    }
    #endregion
    
    #region Data Loading Operations
    /// <summary>
    /// Loads state codes with progress indication
    /// Simulated loading for demonstration
    /// </summary>
    public async Task LoadStateCodesAsync()
    {
        InfoMessageTitle = "Loading State Codes...";
        InfoMessage = "State Codes are loading. Please wait.";

        await SimulateDataLoading();
        await Task.Delay(1000); // Additional delay for UI feedback
    }

    /// <summary>
    /// Loads country codes with progress indication
    /// Simulated loading for demonstration
    /// </summary>
    public async Task LoadCountryCodesAsync()
    {
        InfoMessageTitle = "Loading Country Codes...";
        InfoMessage = "Country Codes are loading. Please wait.";
        
        await SimulateDataLoading();
        await Task.Delay(1000);
    }

    /// <summary>
    /// Loads employee types with progress indication
    /// Simulated loading for demonstration
    /// </summary>
    public async Task LoadEmployeeTypesAsync()
    {
        InfoMessageTitle = "Loading Employee Types...";
        InfoMessage = "Employee Types are loading. Please wait.";
        
        await SimulateDataLoading();
        await Task.Delay(1000);
    }

    /// <summary>
    /// Simulates data loading operation
    /// </summary>
    private async Task SimulateDataLoading()
    {
        await Task.Run(async () =>
        {
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(500);
            }
        });
    }
    #endregion
}