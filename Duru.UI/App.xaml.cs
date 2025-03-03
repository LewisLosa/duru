using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Duru.Application.Extensions;
using Duru.Application.Interfaces;
using Duru.Application.Mappings;
using Duru.Application.Services;
using Duru.UI.ViewModels;

namespace Duru.UI
{
    public partial class App : System.Windows.Application
    {
        public static ServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<MainWindowViewModel>();
            serviceCollection.AddTransient<Views.MainWindow>();
            serviceCollection.AddAutoMapper(typeof(RoomProfile));
            serviceCollection.AddApplicationServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<Views.MainWindow>(); 
            mainWindow.Show();
        }
    }
}