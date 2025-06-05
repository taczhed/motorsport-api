using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Tests.Helpers;

namespace MotorsportApi.Tests.Controllers;

[Collection("ApiCollection")]
public class TracksControllerTests
{
    private readonly HttpClient _client;
    private readonly AuthClient _auth;

    public TracksControllerTests(MotorsportApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _auth = new AuthClient();
    }

    [Fact]
    public async Task GetAllTracks_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/tracks");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tracks = await response.Content.ReadFromJsonAsync<List<TrackDto>>();
        tracks.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetTrackById_ExistingId_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/tracks/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var track = await response.Content.ReadFromJsonAsync<TrackDto>();
        track.Should().NotBeNull();
        track!.Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetTrackById_InvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/tracks/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTrack_AsAdmin_ReturnsCreated()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/tracks");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new TrackInputDto
        {
            Name = "Test Track",
            Location = "Nowhere",
            LengthKm = 4.321
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var track = await response.Content.ReadFromJsonAsync<TrackDto>();
        track!.Name.Should().Be("Test Track");
    }

    [Fact]
    public async Task UpdateTrack_AsAdmin_ReturnsNoContent()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Put, "/api/tracks/1");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new TrackInputDto
        {
            Name = "Updated Track",
            Location = "Updated City",
            LengthKm = 5.432
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateTrack_InvalidId_ReturnsNotFound()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Put, "/api/tracks/999");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new TrackInputDto
        {
            Name = "Ghost Track",
            Location = "Null",
            LengthKm = 0.0
        });

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTrack_AsAdmin_ReturnsNoContent()
    {
        var token = await _auth.GetTokenAsync();

        // najpierw utwórz tor
        var create = new HttpRequestMessage(HttpMethod.Post, "/api/tracks");
        create.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        create.Content = JsonContent.Create(new TrackInputDto
        {
            Name = "DeleteMe Track",
            Location = "Somewhere",
            LengthKm = 3.14
        });

        var created = await _client.SendAsync(create);
        var track = await created.Content.ReadFromJsonAsync<TrackDto>();

        // teraz go usuń
        var delete = new HttpRequestMessage(HttpMethod.Delete, $"/api/tracks/{track!.Id}");
        delete.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(delete);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTrack_InvalidId_ReturnsNotFound()
    {
        var token = await _auth.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/tracks/999");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
