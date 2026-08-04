using Microsoft.EntityFrameworkCore;
using NorthwindService;
using NorthwindService.Services;
using NortwindReporting;
using NortwindReporting.Models;
using OpenAI;
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

Console.WriteLine("================================");
Console.WriteLine(connectionString);
Console.WriteLine("================================");

builder.Services.AddDbContext<NorthwindContext>(option=>
  option.UseSqlServer(connectionString)
);

builder.Services.AddScoped<INorthwindService, NorthwindService.NorthwindService>();
builder.Services.AddScoped<ICacheService,CacheService>();
//builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.Configure<OpenAIOptions>(
    builder.Configuration.GetSection("OpenAI"));

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var apiKey = config["OpenAI:ApiKey"]!;

    return new OpenAIClient(apiKey);
});

builder.Services.AddScoped<IChatService, ChatService>();

/*Testing how singleton service work*/
builder.Services.AddScoped<TestSingletonService>();



var app = builder.Build();
app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
