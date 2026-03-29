using CarsProject.DAL;
using CarsProject.WebApi.Mapping;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using CarsProject.Service;
using CarsProject.Service.Common;
using CarsProject.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<ICarMakeService, CarMakeService>();
builder.Services.AddScoped<ICarMakeRepository, CarMakeRepository>();

builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();

builder.Services.AddScoped<ICarOwnerService, CarOwnerService>();
builder.Services.AddScoped<ICarOwnerRepository, CarOwnerRepository>();

builder.Services.AddScoped<ICarRegistrationService, CarRegistrationService>();
builder.Services.AddScoped<ICarRegistrationRepository, CarRegistrationRepository>();

builder.Services.AddScoped<ICarEngineTypeService, CarEngineTypeService>();
builder.Services.AddScoped<ICarEngineTypeRepository, CarEngineTypeRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CarsProjectMappingProfile).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarsProjectCORS();

builder.Services.AddDbContext<CarsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CarsDbContext"),
        b => b.MigrationsAssembly("CarsProject.DAL")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowViteDev");

app.MapControllers();

app.UseStaticFiles();
app.UseDefaultFiles();
app.MapFallbackToFile("index.html");

app.Run();
