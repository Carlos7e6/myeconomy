using Microsoft.EntityFrameworkCore;
using my_economy_api.Data;
using RepositoryPatern.Interfaces;
using RepositoryPatern;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext with the dependency injection container, configuring it to use SQL Server with the connection string specified in the application's configuration settings.
// This allows the application to interact with the database using Entity Framework Core for data access operations.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DbContext as a scoped service in the dependency injection container, allowing it to be injected into other services and controllers that require database access.
builder.Services.AddScoped<DbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// Register the generic repository interface and its implementation in the dependency injection container, allowing for the use of the repository pattern throughout the application for data access operations.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment()) //To avoid the warnings in tests
{
    app.UseHttpsRedirection();
}

app.UseRouting();     

app.UseCors("AllowLocalhost"); 

app.UseAuthorization();   

app.MapControllers();      

app.Run();

public partial class Program { }
