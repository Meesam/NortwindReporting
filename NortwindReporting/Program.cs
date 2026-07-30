using Microsoft.EntityFrameworkCore;
using NorthwindService;
using NortwindReporting.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

/*Configure Serilog*/
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(option=>
{
    option.Configuration = "localhost:6379";
    option.InstanceName = "NorthwindReporting";
});


var connectionString = builder.Configuration.GetConnectionString("NorthwindConnection");
builder.Services.AddDbContext<NorthwindContext>(option=>
  option.UseSqlServer(connectionString)
);

builder.Services.AddScoped<INorthwindService, NorthwindService.NorthwindService>();
builder.Services.AddScoped<ICacheService,CacheService>();
//builder.Services.AddScoped<ICacheService, RedisCacheService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
