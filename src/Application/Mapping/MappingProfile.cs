using AutoMapper;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Domain.Entities;

namespace MotorsportApi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Car
        CreateMap<Car, CarDto>();
        CreateMap<Car, CarBasicDto>();
        CreateMap<CarInputDto, Car>();

        // Driver
        CreateMap<Driver, DriverDto>();
        CreateMap<Driver, DriverBasicDto>();
        CreateMap<DriverInputDto, Driver>();

        // Track
        CreateMap<Track, TrackDto>();
        CreateMap<TrackInputDto, Track>();

        // DriverRace
        CreateMap<DriverRaceInputDto, DriverRace>();
        CreateMap<DriverRace, DriverRaceDto>()
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver));

        // Race
        CreateMap<RaceInputDto, Race>();
        CreateMap<Race, RaceDto>()
            .ForMember(dest => dest.Track, opt => opt.MapFrom(src => src.Track))
            .ForMember(dest => dest.Drivers, opt => opt.MapFrom(src => src.DriverRaces));
    }
}