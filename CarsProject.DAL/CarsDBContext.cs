using Microsoft.EntityFrameworkCore;

namespace CarsProject.DAL
{
    public class CarsDbContext : DbContext
    {
        public DbSet<Model.CarMake> CarMakes { get; set; }
        public DbSet<Model.CarModel> CarModels { get; set; }
        public DbSet<Model.CarOwner> CarOwners { get; set; }
        public DbSet<Model.CarRegistration> CarRegistrations { get; set; }
        public DbSet<Model.CarEngineType> CarEngineTypes { get; set; }

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
            
            modelBuilder.Entity<CarEngineType>().HasData(
                new CarEngineType { Id = 1, Type = "Petrol", Abrv = "PET" },
                new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DSL" },
                new CarEngineType { Id = 3, Type = "Electric", Abrv = "ELE" },
                new CarEngineType { Id = 4, Type = "Hybrid", Abrv = "HYB" }
            );
        }
    }
}





