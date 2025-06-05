using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Tests.Helpers;

namespace MotorsportApi.Tests.Controllers
{
    [Collection("ApiCollection")]
    public class DriversControllerTests
    {
        private readonly HttpClient _client;
        private readonly AuthClient _auth;

        public DriversControllerTests(MotorsportApiWebApplicationFactory factory)
        {
            var app = new MotorsportApiWebApplicationFactory();

            _client = factory.CreateClient();
            _auth = new AuthClient();
        }

        [Fact]
        public async Task GetAllDrivers_ReturnsOkWithData()
        {
            var response = await _client.GetAsync("/api/drivers");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var drivers = await response.Content.ReadFromJsonAsync<List<DriverDto>>();
            drivers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetDriverById_ExistingId_ReturnsDriver()
        {
            var response = await _client.GetAsync("/api/drivers/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var driver = await response.Content.ReadFromJsonAsync<DriverDto>();
            driver.Should().NotBeNull();
            driver!.Name.Should().Be("Lewis Hamilton");
        }

        [Fact]
        public async Task GetDriverById_InvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/drivers/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateDriver_AsAdmin_ReturnsCreated()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/drivers");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new DriverInputDto
            {
                Name = "Test Driver",
                Age = 30
            });

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var driver = await response.Content.ReadFromJsonAsync<DriverDto>();
            driver!.Name.Should().Be("Test Driver");
        }

        [Fact]
        public async Task UpdateDriver_ValidId_ReturnsNoContent()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/drivers/2");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new DriverInputDto
            {
                Name = "Updated Verstappen",
                Age = 39
            });

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateDriver_InvalidId_ReturnsNotFound()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/drivers/999");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new DriverInputDto
            {
                Name = "Ghost Driver",
                Age = 99
            });

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteDriver_ValidId_ReturnsNoContent()
        {
            var token = await _auth.GetTokenAsync();

            var create = new HttpRequestMessage(HttpMethod.Post, "/api/drivers");
            create.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            create.Content = JsonContent.Create(new DriverInputDto
            {
                Name = "Temp Driver",
                Age = 33
            });

            var created = await _client.SendAsync(create);
            var driver = await created.Content.ReadFromJsonAsync<DriverDto>();

            var delete = new HttpRequestMessage(HttpMethod.Delete, $"/api/drivers/{driver!.Id}");
            delete.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(delete);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteDriver_InvalidId_ReturnsNotFound()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/drivers/999");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
