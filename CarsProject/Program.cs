using AutoMapper;
using CarsProject;
using CarsProject.DAL;
using CarsProject.Mapping;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using CarsProject.Service;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Ninject.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // frontend port
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

//Ninject
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());


builder.Host.ConfigureContainer<IKernel>(kernel =>
{
    kernel.Bind<ICarRegistrationService>().To<CarRegistrationService>();
    kernel.Bind<ICarModelService>().To<CarModelService>();
    kernel.Bind<ICarEngineTypeService>().To<CarEngineTypeService>();
    kernel.Bind<ICarOwnerService>().To<CarOwnerService>();
    kernel.Bind<ICarMakeService>().To<CarMakeService>();
    kernel.Bind<ICarMakeRepository>().To<CarMakeRepository>();
    kernel.Bind<ICarModelRepository>().To<CarModelRepository>();
    kernel.Bind<ICarOwnerRepository>().To<CarOwnerRepository>();
    kernel.Bind<ICarRegistrationRepository>().To<CarRegistrationRepository>();
    kernel.Bind<ICarEngineTypeRepository>().To<CarEngineTypeRepository>();
    kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
    
});

//Automapper configuration
builder.Services.AddAutoMapper(typeof(CarsProjectMappingProfile).Assembly);


builder.Services.AddControllers();
// Swagger configuration
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

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();


