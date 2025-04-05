using CarsProject;
using CarsProject.DAL;
using CarsProject.Mapping;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Ninject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//NINJECT
var kernel = NinjectConfig.CreateKernel();

builder.Services.AddSingleton<IKernel>(kernel);

builder.Services.AddScoped<ICarMakeRepository, CarMakeRepository>();
builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
builder.Services.AddScoped<ICarOwnerRepository, CarOwnerRepository>();
builder.Services.AddScoped<ICarRegistrationRepository, CarRegistrationRepository>();
builder.Services.AddScoped<ICarEngineTypeRepository, CarEngineTypeRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CarsProjectMappingProfile).Assembly);

builder.Services.AddDbContext<CarsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CarsProject.DAL"))); 

var app = builder.Build();

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
