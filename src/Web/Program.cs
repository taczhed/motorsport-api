using Microsoft.EntityFrameworkCore;
using MotorsportApi.Application.Mapping;
using MotorsportApi.Infrastructure;
using MotorsportApi.Infrastructure.Persistence;
using MotorsportApi.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MotorsportDb"));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Data seed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = new DataSeeder(dbContext);
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestCountingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
