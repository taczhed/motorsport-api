namespace MotorsportApi.Domain.Entities;

public class Race
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public int TrackId { get; set; }
    public Track? Track { get; set; }

    public ICollection<DriverRace> DriverRaces { get; set; } = new List<DriverRace>();
}