using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarsProject.Model;

namespace CarsProject.DAL
{
    public class CarsDbContext : DbContext
    {
        public DbSet<CarsProject.Model.CarMake> CarMakes { get; set; }
        public DbSet<CarsProject.Model.CarModel> CarModels { get; set; }
        public DbSet<CarsProject.Model.CarOwner> CarOwners { get; set; }
        public DbSet<CarsProject.Model.CarRegistration> CarRegistrations { get; set; }
        public DbSet<CarsProject.Model.CarEngineType> CarEngineTypes { get; set; }

        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options) { }
    }
}
