using CarsProject.DAL;
using CarsProject.Extensions;
using CarsProject.Mapping;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using CarsProject.Service;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Ninject.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);


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

builder.Services.AddAutoMapper(typeof(CarsProjectMappingProfile).Assembly);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarsProjectCORS();


builder.Services.AddDbContext<CarsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarsProjectContext"),
        b => b.MigrationsAssembly("CarsProject.DAL")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapControllers();

app.UseStaticFiles();
app.UseDefaultFiles();
app.MapFallbackToFile("index.html");



app.Run();


