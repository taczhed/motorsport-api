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
            // Get token
            var token = await _auth.GetTokenAsync();

            Console.WriteLine(token.ToString());

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
    }
}
