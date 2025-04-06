using CarsProject;
using CarsProject.DAL;
using CarsProject.Mapping;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using CarsProject.Service;
using Microsoft.EntityFrameworkCore;
using Ninject;


var builder = WebApplication.CreateBuilder(args);

//Ninject configuration
var kernel = NinjectConfig.CreateKernel();

builder.Services.AddSingleton<IKernel>(kernel);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICarMakeService,CarMakeService>();
builder.Services.AddScoped<ICarMakeRepository, CarMakeRepository>();
builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
builder.Services.AddScoped<ICarOwnerRepository, CarOwnerRepository>();
builder.Services.AddScoped<ICarRegistrationRepository, CarRegistrationRepository>();
builder.Services.AddScoped<ICarEngineTypeRepository, CarEngineTypeRepository>();

//Automapper configuration
builder.Services.AddAutoMapper(typeof(CarsProjectMappingProfile).Assembly);

builder.Services.AddControllers();
// Swagger konfiguracija
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dodavanje DbContext za povezivanje s bazom podataka
builder.Services.AddDbContext<CarsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarsDbContext"),
        b => b.MigrationsAssembly("CarsProject.DAL")));

var app = builder.Build();

// Swagger middleware za razvojnu okolinu
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
