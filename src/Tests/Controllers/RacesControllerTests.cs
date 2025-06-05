using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Tests.Helpers;

namespace MotorsportApi.Tests.Controllers;

[Collection("ApiCollection")]
public class RacesControllerTests
{
    private readonly HttpClient _client;
    private readonly AuthClient _auth;

    public RacesControllerTests(MotorsportApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _auth = new AuthClient();
    }

    [Fact]
    public async Task GetRaces_ReturnsOkWithData()
    {
        var response = await _client.GetAsync("/api/races");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var races = await response.Content.ReadFromJsonAsync<List<RaceDto>>();
        races.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetRaceById_ExistingId_ReturnsRace()
    {
        var response = await _client.GetAsync("/api/races/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var race = await response.Content.ReadFromJsonAsync<RaceDto>();
        race.Should().NotBeNull();
        race!.Track.Should().NotBeNull();
        race.Drivers.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRaceById_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/races/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRace_AsAdmin_ReturnsCreated()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/races");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new RaceInputDto
        {
            Name = "Test GP",
            Date = DateTime.UtcNow.AddDays(30),
            TrackId = 1
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var race = await response.Content.ReadFromJsonAsync<RaceDto>();
        race!.Name.Should().Be("Test GP");
    }

    [Fact]
    public async Task UpdateRace_AsAdmin_ReturnsNoContent()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Put, "/api/races/1");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new RaceInputDto
        {
            Name = "Updated GP",
            Date = DateTime.UtcNow.AddMonths(1),
            TrackId = 1
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteRace_AsAdmin_ReturnsNoContent()
    {
        var token = await _auth.GetTokenAsync();

        var create = new HttpRequestMessage(HttpMethod.Post, "/api/races");
        create.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        create.Content = JsonContent.Create(new RaceInputDto
        {
            Name = "Temp GP",
            Date = DateTime.UtcNow.AddDays(60),
            TrackId = 1
        });
        var created = await _client.SendAsync(create);
        var race = await created.Content.ReadFromJsonAsync<RaceDto>();

        var delete = new HttpRequestMessage(HttpMethod.Delete, $"/api/races/{race!.Id}");
        delete.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(delete);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AddDriverToRace_AsAdmin_ReturnsOk()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/races/1/drivers");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new DriverRaceInputDto
        {
            DriverId = 4,
            Position = 3,
            Time = TimeSpan.FromMinutes(88)
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateDriverResult_AsAdmin_ReturnsNoContent()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Put, "/api/races/1/drivers/1");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new DriverRaceInputDto
        {
            Position = 1,
            Time = TimeSpan.FromMinutes(84)
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveDriverFromRace_AsAdmin_ReturnsNoContent()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/races/1/drivers/1");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
