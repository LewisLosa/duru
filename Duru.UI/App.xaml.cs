using System.IO;
using System.Windows;
using Duru.Data;
using Duru.Data.Repositories;
using Duru.Data.Services;
using Duru.UI.ViewModels;
using Duru.UI.ViewModels.Pages;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// Add view model namespaces
// For Path

namespace Duru.UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        try
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Database initialization failed: {ex.Message}\n\nApplication will exit.", "Database Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }
        // ----------------------------------------


        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var dbFolder = AppDomain.CurrentDomain.BaseDirectory;
        Directory.CreateDirectory(dbFolder);
        var dbPath = Path.Combine(dbFolder, "duru_hotel.db");

        services.AddDbContext<ApplicationDbContext>(
            options =>
                options.UseSqlite($"Data Source={dbPath}"));

        // --- Repositories ---
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // --- Services ---
        services.AddScoped<IRoomService, RoomService>();

        // --- ViewModels ---
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<HomePageViewModel>();
        services.AddTransient<RoomManagementViewModel>();
        services.AddTransient<GuestManagementViewModel>();
        services.AddTransient<ReservationManagementViewModel>();
        services.AddTransient<PaymentManagementViewModel>();
        services.AddTransient<ReportingViewModel>();
        services.AddTransient<DatabaseManagementPageViewModel>();
        services.AddTransient<IntegrationsViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<UserProfileViewModel>();


        // --- Configure MahApps Dialogs ---
        services.AddSingleton<IDialogCoordinator, DialogCoordinator>(); // Register DialogCoordinator

        // --- Configure MainWindow ---
        services.AddTransient<MainWindow>(); // Register MainWindow
    }
}