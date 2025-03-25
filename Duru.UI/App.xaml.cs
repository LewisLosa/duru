using System.Windows;
using Duru.UI.Utils.AppSettings;

namespace Duru.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Set the DataDirectory for Entity Framework
        string path = Environment.CurrentDirectory;
        path = path.Replace(@"\bin\Debug", "");
        path += @"\DuruDB\";

        AppDomain.CurrentDomain.SetData("DataDirectory", path);

        // Load Application Settings
        AppSettings.Instance.LoadSettings();
    }
}