using MotorsportApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MotorsportApi.Infrastructure.Persistence;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Drivers.AnyAsync()) return; // Skip - seed exists :)

        var drivers = new List<Driver>
        {
            new() { Name = "Lewis Hamilton", Age = 38 },
            new() { Name = "Max Verstappen", Age = 26 },
            new() { Name = "Fernando Alosno", Age = 42 },
            new() { Name = "Robert Kubica", Age = 40 },
        };

        var cars = new List<Car>
        {
            new() { Brand = "Ferrari", Model = "SF25", Number = "44", Driver = drivers[0] },
            new() { Brand = "Red Bull", Model = "RB19", Number = "1", Driver = drivers[1] },
            new() { Brand = "Aston Martin", Model = "AMR23", Number = "14", Driver = drivers[2] },
        };

        var tracks = new List<Track>
        {
            new() { Name = "Silverstone", Location = "UK", LengthKm = 5.891 },
            new() { Name = "Spa-Francorchamps", Location = "Belgium", LengthKm = 7.004 },
        };

        var races = new List<Race>
        {
            new() { Name = "British GP", Date = new DateTime(2024, 7, 7), Track = tracks[0] },
            new() { Name = "Belgian GP", Date = new DateTime(2024, 8, 1), Track = tracks[1] },
        };

        var driverRaces = new List<DriverRace>
        {
            new() { Driver = drivers[0], Race = races[0], Position = 1, Time = TimeSpan.FromMinutes(85) },
            new() { Driver = drivers[1], Race = races[0], Position = 2, Time = TimeSpan.FromMinutes(85.5) },
            new() { Driver = drivers[2], Race = races[1], Position = 2, Time = TimeSpan.FromMinutes(87) },
            new() { Driver = drivers[3], Race = races[1], Position = 1, Time = TimeSpan.FromMinutes(83) },
        };

        _context.AddRange(drivers);
        _context.AddRange(cars);
        _context.AddRange(tracks);
        _context.AddRange(races);
        _context.AddRange(driverRaces);

        await _context.SaveChangesAsync();
    }
}
