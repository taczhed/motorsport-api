using AutoMapper;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Domain.Entities;

namespace MotorsportApi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Car, CarDto>().ReverseMap();
        CreateMap<Driver, DriverDto>().ReverseMap();
        CreateMap<Track, TrackDto>().ReverseMap();
        CreateMap<DriverRace, DriverRaceDto>().ReverseMap();
        CreateMap<Race, RaceDto>()
            .ForMember(dest => dest.Drivers, opt => opt.MapFrom(src =>
                src.DriverRaces.Select(dr => new DriverRaceDto
                {
                    Id = dr.Driver.Id,
                    Name = dr.Driver.Name,
                    Position = dr.Position,
                    Time = dr.Time
                })));
    }
}