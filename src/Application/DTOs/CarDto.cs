namespace MotorsportApi.Application.DTOs;

public class CarDto
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public int DriverId { get; set; }
}
