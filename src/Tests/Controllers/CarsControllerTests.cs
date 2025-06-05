using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Tests.Helpers;

namespace MotorsportApi.Tests.Controllers
{
    public class CarsControllerTests
    {
        private readonly HttpClient _client;
        private readonly AuthClient _auth;

        public CarsControllerTests()
        {
            var app = new MotorsportApiWebApplicationFactory();

            _client = app.CreateClient();
            _auth = new AuthClient();
        }

        [Fact]
        public async Task GetCars_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/cars");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var cars = await response.Content.ReadFromJsonAsync<List<CarDto>>();
            cars.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetCarById_ExistingId_ReturnsCar()
        {
            var response = await _client.GetAsync("/api/cars/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var car = await response.Content.ReadFromJsonAsync<CarDto>();
            car.Should().NotBeNull();
            car!.Driver.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCarById_InvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/cars/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateCar_AsAdmin_ReturnsCreated()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/cars");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new CarInputDto
            {
                Brand = "TestBrand",
                Model = "TestModel",
                Number = "99",
                DriverId = 4
            });

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var car = await response.Content.ReadFromJsonAsync<CarDto>();
            car!.Brand.Should().Be("TestBrand");
        }

        [Fact]
        public async Task UpdateCar_AsAdmin_ReturnsNoContent()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/cars/1");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new CarInputDto
            {
                Brand = "UpdatedBrand",
                Model = "UpdatedModel",
                Number = "999",
                DriverId = 5
            });

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateCar_WithNonExistingId_ReturnsNotFound()
        {
            var token = await _auth.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/cars/999");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new CarInputDto
            {
                Brand = "UpdatedBrand",
                Model = "UpdatedModel",
                Number = "999",
                DriverId = 999
            });

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteCar_AsAdmin_ReturnsNoContent()
        {
            var token = await _auth.GetTokenAsync();

            var create = new HttpRequestMessage(HttpMethod.Post, "/api/cars");
            create.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            create.Content = JsonContent.Create(new CarInputDto
            {
                Brand = "TestBrand",
                Model = "TestModel",
                Number = "99",
                DriverId = 4
            });
            var createdResponse = await _client.SendAsync(create);
            var car = await createdResponse.Content.ReadFromJsonAsync<CarDto>();

            Console.WriteLine($"--- ${createdResponse.StatusCode} ---");

            var delete = new HttpRequestMessage(HttpMethod.Delete, $"/api/cars/{car!.Id}");
            delete.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(delete);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}