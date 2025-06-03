namespace MotorsportApi.Application.DTOs;

public class CarInputDto
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Number { get; set; }
    public int DriverId { get; set; }
}

public class CarBasicDto
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Number { get; set; }
}

public class CarDto : CarBasicDto
{
    public DriverBasicDto Driver { get; set; }
}
