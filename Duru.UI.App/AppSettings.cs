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
    private Employee _employeeEntity = new Employee();
    private int _infoMessageTimeout;
    private string _emailDomain;
    #endregion

    #region Public Properties

    public Employee EmployeeEntity
    {
        get => _employeeEntity;
        set
        {
            _employeeEntity = value;
            RaisePropertyChanged("EmployeeEntity");
        }
    }
    
    public int InfoMessageTimeout
    {
        get { return _infoMessageTimeout; }
        set {
            _infoMessageTimeout = value;
            RaisePropertyChanged("InfoMessageTimeout");
        }
    }

    public string EmailDomain
    {
        get { return _emailDomain; }
        set {
            _emailDomain = value;
            RaisePropertyChanged("EmailDomain");
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