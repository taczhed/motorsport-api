namespace MotorsportApi.Domain.Entities;

public class DriverRace
{
    public int DriverId { get; set; }
    public Driver? Driver { get; set; }

    public int RaceId { get; set; }
    public Race? Race { get; set; }

    public int Position { get; set; } 
    public TimeSpan? Time { get; set; }
}