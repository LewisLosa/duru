using Duru.Library;
using System.Timers;
using Duru.Library.ViewModels;
using Duru.UI.Data;
using Duru.UI.Data.Entities;
using Timer = System.Timers.Timer;

namespace Duru.UI.ViewModel
{
  public class MainWindowViewModel : ViewModelBase
  {
    #region Private Variables
    private const int SECONDS = 500;

    private string _loginMenuHeader = "Login";
    private string _statusMessage;

    private bool _isInfoMessageVisible = true;
    private string _infoMessage;
    private string _infoMessageTitle;

    private Timer _infoMessageTimer = null;
    private int _infoMessageTimeout;

    private Room _roomEntity = new Room();
    #endregion

    #region Public Properties
    public string LoginMenuHeader
    {
      get { return _loginMenuHeader; }
      set {
        _loginMenuHeader = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }

    public string StatusMessage
    {
      get { return _statusMessage; }
      set {
        _statusMessage = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }

    public bool IsInfoMessageVisible
    {
      get { return _isInfoMessageVisible; }
      set {
        _isInfoMessageVisible = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }

    public string InfoMessage
    {
      get { return _infoMessage; }
      set {
        _infoMessage = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }

    public string InfoMessageTitle
    {
      get { return _infoMessageTitle; }
      set {
        _infoMessageTitle = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }

    public int InfoMessageTimeout
    {
      get { return _infoMessageTimeout; }
      set {
        _infoMessageTimeout = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }
    
    public Room RoomEntity
    {
      get { return _roomEntity; }
      set {
        _roomEntity = value;
        // TODO: Güncelleme eventi eklenecek.
      }
    }
    #endregion

    #region ClearInfoMessage Method
    public void ClearInfoMessages()
    {
      InfoMessage = string.Empty;
      InfoMessageTitle = string.Empty;
      IsInfoMessageVisible = false;
    }
    #endregion

    #region Message Timer Methods
    public virtual void CreateInfoMessageTimer()
    {
      if (_infoMessageTimer == null) {
        // Create informational message timer
        _infoMessageTimer = new Timer(_infoMessageTimeout);
        // Connect to an Elapsed event
        _infoMessageTimer.Elapsed += _MessageTimer_Elapsed;
      }
      _infoMessageTimer.AutoReset = false;
      _infoMessageTimer.Enabled = true;
      IsInfoMessageVisible = true;
    }

    private void _MessageTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      IsInfoMessageVisible = false;
    }
    #endregion

    #region LoadStateCodes Method
    public void LoadStateCodes()
    {
      // TODO: Write code to load state codes here
      System.Threading.Thread.Sleep(SECONDS);
    }
    #endregion

    #region LoadCountryCodes Method
    public void LoadCountryCodes()
    {
      // TODO: Write code to load country codes here
      System.Threading.Thread.Sleep(SECONDS);
    }
    #endregion

    #region LoadEmployeeTypes Method
    public void LoadEmployeeTypes()
    {
      // TODO: Write code to load employee types here
      System.Threading.Thread.Sleep(SECONDS);
    }
    #endregion
  }
}
