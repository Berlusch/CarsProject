using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.DAL
{
    public class CarsDbContext : DbContext
    {
        public DbSet<CarMake> CarMakes { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<CarOwner> CarOwners { get; set; }
        public DbSet<CarRegistration> CarRegistrations { get; set; }
        public DbSet<CarEngineType> CarEngineTypes { get; set; }

        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options) { }
    }
}
