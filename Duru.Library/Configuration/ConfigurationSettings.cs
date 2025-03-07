using Microsoft.Extensions.Configuration;
using System;

namespace Duru.Library.Configuration
{
    /// <summary>
    /// This class holds global data for this application
    /// </summary>
    public class ConfigurationSettings : CommonBase
    {
        private readonly IConfiguration _configuration;

        #region LoadSettings Method
        public virtual void LoadSettings()
        {
            // TODO: Load any common application settings here
        }
        #endregion

        #region GetSetting Method
        public T GetSetting<T>(string key, T defaultValue)
        {
            string value = _configuration[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
        #endregion
    }
}