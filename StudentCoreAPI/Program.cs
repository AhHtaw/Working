using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentCoreAPI.Models;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
var builder = WebApplication.CreateBuilder(args);

// Apply logging
builder.Logging.ClearProviders();

var logPath = config.GetValue<string>("Logging:FilePath");

var logger = new LoggerConfiguration()
    .WriteTo.File(path: logPath,
    rollOnFileSizeLimit: true,
    fileSizeLimitBytes: Convert.ToInt32(config.GetValue<string>("Logging:MaxLogFileSize")))
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var myOrigins = "_myOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200");
        });
});

//IConfiguration configuration;
//configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddDbContext<IpDbfirstContext>
    (option => option.UseSqlServer(config.GetConnectionString("StudentConnectionString")));

var app = builder.Build();
app.UseCors(myOrigins);

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
