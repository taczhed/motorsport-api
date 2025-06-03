namespace MotorsportApi.Application.DTOs;

public class DriverRaceInputDto
{
    public int DriverId { get; set; }
    public string DriverName { get; set; }
    public int? Position { get; set; }
    public TimeSpan? Time { get; set; }
}

public class DriverRaceDto
{
    public DriverBasicDto Driver { get; set; }
    public int? Position { get; set; }
    public TimeSpan? Time { get; set; }
}