using CargoApi.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Read the configuration from appsettings.json

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add CORS configuration
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowOrigin", builder =>
//    {
//        builder
//           .AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
       builder.WithOrigins("https://pwswarehouse.azurewebsites.net") //https://pwswarehouse.azurewebsites.net
      // builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Set up the database context with the connection string
var connectionString = configuration.GetConnectionString("MyDatabaseConnection");
builder.Services.AddDbContext<PRIORITY_WWDContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// ... Other service configurations ...

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
// Use CORS middlewar
app.UseCors("AllowOrigin");

app.UseAuthorization();





app.MapControllers();

app.Run();



//using CargoApi.Models;
//using Microsoft.EntityFrameworkCore;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.
//        builder.Services.AddControllers();

//        // Read the configuration from appsettings.json
//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(builder.Environment.ContentRootPath)
//            .AddJsonFile("appsettings.json")
//            .Build();

//        // Set up the database context with the connection string
//        var connectionString = configuration.GetConnectionString("MyDatabaseConnection");
//        builder.Services.AddDbContext<PRTYCTX>(options =>
//        {
//            options.UseSqlServer(connectionString);
//        });

//        // ... Other service configurations ...
//        // Add CORS configuration
//        builder.Services.AddCors(options =>
//        {
//            options.AddPolicy("AllowOrigin", builder =>
//            {
//                builder
//                    .AllowAnyOrigin()
//                    .AllowAnyHeader()
//                    .AllowAnyMethod();
//            });
//        });

//        var app = builder.Build();

//        // Configure the HTTP request pipeline.
//        if (app.Environment.IsDevelopment())
//        {
//            app.UseSwagger();
//            app.UseSwaggerUI();
//        }

//        app.UseHttpsRedirection();
//        app.UseAuthorization();

//        // Use CORS middleware
//        app.UseCors("AllowOrigin");

//        app.MapControllers();

//        app.Run();
//    }
//}