using Microsoft.EntityFrameworkCore;
using MotorsportApi.Domain.Entities;

namespace MotorsportApi.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Race> Races => Set<Race>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<DriverRace> DriverRaces => Set<DriverRace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Driver <-> Car (1:1)
        modelBuilder.Entity<Driver>()
            .HasOne(d => d.Car)
            .WithOne(c => c.Driver)
            .HasForeignKey<Car>(c => c.DriverId);

        // Driver <-> Race (many:many)
        modelBuilder.Entity<DriverRace>()
            .HasKey(dr => new { dr.DriverId, dr.RaceId });

        modelBuilder.Entity<DriverRace>()
            .HasOne(dr => dr.Driver)
            .WithMany(d => d.DriverRaces)
            .HasForeignKey(dr => dr.DriverId);

        modelBuilder.Entity<DriverRace>()
            .HasOne(dr => dr.Race)
            .WithMany(r => r.DriverRaces)
            .HasForeignKey(dr => dr.RaceId);
    }
}
