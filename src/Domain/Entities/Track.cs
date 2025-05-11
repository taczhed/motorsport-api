namespace MotorsportApi.Domain.Entities;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double LengthKm { get; set; }

    public ICollection<Race> Races { get; set; } = new List<Race>();
}