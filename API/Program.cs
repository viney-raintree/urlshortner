using API.Models;
using API.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ShortURLDatabaseSettings>(
                builder.Configuration.GetSection(nameof(ShortURLDatabaseSettings)));

builder.Services.AddSingleton<IShortURLDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<ShortURLDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("ShortURLDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IShortURLService, ShortURLService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
