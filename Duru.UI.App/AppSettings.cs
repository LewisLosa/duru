using Duru.Library.Configuration;
using Duru.UI.Data.Entities;

namespace Duru.UI.App;

public class AppSettings : ConfigurationSettings
{

    #region Instance Fields
    private static AppSettings _instance;

    public static AppSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppSettings();
            }
            return _instance;
        }
        set => _instance = value;
    }
    #endregion

    #region Private Properties
    private Room _roomEntity = new Room();
    private int _infoMessageTimeout;
    private string _emailDomain;
    #endregion

    #region Public Properties

    public Room RoomEntity
    {
        get => _roomEntity;
        set
        {
            _roomEntity = value;
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

    public string EmailDomain
    {
        get { return _emailDomain; }
        set {
            _emailDomain = value;
            // TODO: Güncelleme eventi eklenecek.
        }
    }
    #endregion

    #region LoadSettings Method
    public override void LoadSettings()
    {
        InfoMessageTimeout = GetSetting<int>("InfoMessageTimeout", 1200);
        EmailDomain = GetSetting<string>("EmailDomain", "");
    }
    #endregion
    

}