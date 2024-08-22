using Microsoft.EntityFrameworkCore;
using StudentCoreAPI.Models;

var builder = WebApplication.CreateBuilder(args);

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


IConfiguration configuration;
configuration=new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddDbContext<IpDbfirstContext>
    (option => option.UseSqlServer(configuration.GetConnectionString("StudentConnectionString")));

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
