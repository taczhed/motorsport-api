namespace MotorsportApi.Application.DTOs;

public class TrackInputDto
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double LengthKm { get; set; }
}

public class TrackDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public double LengthKm { get; set; }
}