namespace MotorsportApi.Application.DTOs;

public class DriverRaceDto
{
    public int Id { get; set; }         // DriverId
    public string Name { get; set; }    // DriverName
    public int? Position { get; set; }      
    public TimeSpan? Time { get; set; }      
}
