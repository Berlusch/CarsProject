using CarsProject;
using CarsProject.DAL;
using CarsProject.Mapping;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Ninject;


var builder = WebApplication.CreateBuilder(args);

var kernel = NinjectConfig.CreateKernel();

builder.Services.AddSingleton<IKernel>(kernel);

// Ostale registracije servisa
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICarMakeRepository, CarMakeRepository>();
builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
builder.Services.AddScoped<ICarOwnerRepository, CarOwnerRepository>();
builder.Services.AddScoped<ICarRegistrationRepository, CarRegistrationRepository>();
builder.Services.AddScoped<ICarEngineTypeRepository, CarEngineTypeRepository>();

builder.Services.AddControllers();
// Swagger konfiguracija
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CarsProjectMappingProfile).Assembly);

// Dodavanje DbContext za povezivanje s bazom podataka
builder.Services.AddDbContext<CarsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
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
