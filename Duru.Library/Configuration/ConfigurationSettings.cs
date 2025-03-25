using System.Configuration;
namespace Duru.Library.Configuration
{
    /// <summary>
    /// This class holds global data for this application
    /// </summary>
    public class ConfigurationSettings : CommonBase
    {
        #region LoadSettings Method
        public virtual void LoadSettings()
        {
            // TODO: Load any common application settings here
        }
        #endregion

        #region GetSetting Method
        public T GetSetting<T>(string key, T defaultValue)
        {
            T ret = default(T);
            string value;

            value = ConfigurationManager.AppSettings[key] ?? string.Empty;
            if (string.IsNullOrEmpty(value)) {
                ret = (T)defaultValue;
            }
            else {
                ret = (T)Convert.ChangeType(value, typeof(T));
            }

            return ret;
        }
        #endregion
    }
}