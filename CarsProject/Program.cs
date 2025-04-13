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

//Ninject
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());


builder.Host.ConfigureContainer<IKernel>(kernel =>
{
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


