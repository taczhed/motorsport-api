using MotorsportApi.Infrastructure;
using MotorsportApi.Domain.Entities;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Car> GetCars(ApplicationDbContext dbContext) => dbContext.Cars;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Driver> GetDrivers(ApplicationDbContext dbContext) => dbContext.Drivers;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Track> GetTracks(ApplicationDbContext dbContext) => dbContext.Tracks;


    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Race> GetRaces(ApplicationDbContext dbContext) => dbContext.Races;
}