using Microsoft.EntityFrameworkCore;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:n relationships

            modelBuilder.Entity<CarModel>().HasOne(g => g.CarMake);           
            //modelBuilder.Entity<CarModel>().HasOne(g => g.CarEngineType)
            //.WithMany();  
            modelBuilder.Entity<CarRegistration>().HasOne(g => g.CarOwner);
            modelBuilder.Entity<CarRegistration>().HasOne(g => g.CarModel);

        }
    }

    

    }
