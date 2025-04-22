using Duru.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Duru.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Payment> Payments => Set<Payment>();

    // Optional: Configure here if not using DI for options (less flexible)
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //          optionsBuilder.UseSqlite("Data Source=duru_hotel.db");
    //     }
    // }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Configure Precision for Decimals ---
        modelBuilder.Entity<Room>()
            .Property(r => r.PricePerNight)
            .HasColumnType("decimal(18, 2)"); // Example precision

        modelBuilder.Entity<Reservation>()
            .Property(r => r.TotalPrice)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Payment>()
            .Property(p => p.AmountPaid)
            .HasColumnType("decimal(18, 2)");

        // --- Configure Unique Constraints ---
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomNumber)
            .IsUnique();

        modelBuilder.Entity<Guest>()
            .HasIndex(g => g.Email)
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL"); // Unique only if email is provided


        // --- Configure Relationships (Fluent API - optional, conventions often suffice) ---
        // Example: Define delete behavior if needed (default is usually Cascade for required, ClientSetNull for optional)
        // modelBuilder.Entity<Reservation>()
        //     .HasOne(r => r.Guest)
        //     .WithMany(g => g.Reservations)
        //     .HasForeignKey(r => r.GuestId)
        //     .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Guest if they have reservations

        // --- Configure Enum to String Conversion (Optional - default is int) ---
        // If you want enums stored as strings in the DB (more readable but less efficient)
        /*
        modelBuilder.Entity<Room>()
            .Property(r => r.Status)
            .HasConversion<string>();
        modelBuilder.Entity<Room>()
            .Property(r => r.RoomType)
            .HasConversion<string>();
        modelBuilder.Entity<Reservation>()
            .Property(r => r.Status)
            .HasConversion<string>();
        modelBuilder.Entity<Payment>()
            .Property(p => p.Status)
            .HasConversion<string>();
        */

        // --- Seed Initial Data (Example) ---
        // modelBuilder.Entity<RoomType>().HasData(
        //     new RoomType { Id = 1, Name = "Single" },
        //     new RoomType { Id = 2, Name = "Double" }
        // );
    }

    // Optional: Override SaveChangesAsync to automatically update timestamps
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(
                e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added) ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges() // Also override the synchronous version
    {
        // Apply the same logic as SaveChangesAsync
        var entries = ChangeTracker
            .Entries()
            .Where(
                e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added) ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }

        return base.SaveChanges();
    }
}