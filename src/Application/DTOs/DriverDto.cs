namespace MotorsportApi.Application.DTOs;

public class DriverInputDto
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class DriverBasicDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class DriverDto : DriverBasicDto
{
    public CarBasicDto Car { get; set; }
}