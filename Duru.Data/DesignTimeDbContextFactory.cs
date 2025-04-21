// Duru.Data/DesignTimeDbContextFactory.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
// For AppDomain, Environment

// For Path

namespace Duru.Data;

/// <summary>
///     This factory is used by the EF Core tools (like Add-Migration) at design time
///     to create instances of the ApplicationDbContext. It provides the necessary
///     DbContextOptions, specifically the connection string.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // --- Configure DbContext Options for Design Time ---
        // Use the *same logic* for determining the database path as in App.xaml.cs
        // to ensure migrations target the correct development database file.

        // Option 1: Relative to executing assembly (might be tools path, test carefully)
        var dbFolder = AppDomain.CurrentDomain.BaseDirectory;

        // Option 2: Specific known location (like user's AppData, safer if Option 1 fails)
        // string dbFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DuruHotelApp_Design");
        // Directory.CreateDirectory(dbFolder); // Ensure it exists

        var dbPath = Path.Combine(dbFolder, "duru_hotel.db"); // Use the same DB name

        // Add console output to see which path is being used during migration generation
        Console.WriteLine($"[DesignTimeDbContextFactory] Using database path: {dbPath}");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}