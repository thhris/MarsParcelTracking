using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Application.Services;
using MarsParcelTracking.Infrastructure.Repos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Retain name for enums in JSON instead of numbers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register Services and Repositories
builder.Services.AddSingleton<IParcelRepository, ParcelRepository>();
builder.Services.AddTransient<IParcelService, ParcelService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
