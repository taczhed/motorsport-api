namespace MotorsportApi.Application.DTOs;

public class RaceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public TrackDto Track { get; set; }

    public List<DriverRaceDto> Drivers { get; set; }
}