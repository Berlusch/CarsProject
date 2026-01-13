using Microsoft.EntityFrameworkCore;

namespace CarsProject.DAL
{
    public class CarsDbContext : DbContext
    {
        public DbSet<CarsProject.WebApi.CarMake> CarMakes { get; set; }
        public DbSet<CarsProject.WebApi.CarModel> CarModels { get; set; }
        public DbSet<CarsProject.WebApi.CarOwner> CarOwners { get; set; }
        public DbSet<CarsProject.WebApi.CarRegistration> CarRegistrations { get; set; }
        public DbSet<CarsProject.WebApi.CarEngineType> CarEngineTypes { get; set; }

        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<CarMake>();
            modelBuilder.Entity<CarModel>();
            modelBuilder.Entity<CarOwner>();
            modelBuilder.Entity<CarRegistration>();
            modelBuilder.Entity<CarEngineType>();
            
            modelBuilder.Entity<CarModel>().HasOne(g => g.CarMake);
            modelBuilder.Entity<CarModel>().HasOne(g => g.CarEngineType)
                .WithMany();

            modelBuilder.Entity<CarRegistration>().HasOne(g => g.CarOwner);
            modelBuilder.Entity<CarRegistration>().HasOne(g => g.CarModel);
        }
    }
}

    

    
