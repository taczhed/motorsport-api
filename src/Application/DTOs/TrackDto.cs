namespace MotorsportApi.Application.DTOs;

public class TrackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double LengthKm { get; set; }
}