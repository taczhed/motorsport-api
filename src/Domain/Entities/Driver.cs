namespace MotorsportApi.Domain.Entities;

public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public Car? Car { get; set; }

    public ICollection<DriverRace> DriverRaces { get; set; } = new List<DriverRace>();
}