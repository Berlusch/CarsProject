using CarsProject.Model;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarMake>();
            modelBuilder.Entity<CarModel>();
            modelBuilder.Entity<CarOwner>();
            modelBuilder.Entity<CarRegistration>();
            modelBuilder.Entity<CarEngineType>();

            modelBuilder.Entity<CarModel>()
                .HasOne(cm => cm.CarMake)
                .WithMany()
                .HasForeignKey(cm => cm.CarMakeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarModel>()
                .HasOne(cm => cm.CarEngineType)
                .WithMany()
                .HasForeignKey(cm => cm.CarEngineTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarRegistration>()
                .HasOne(cr => cr.CarOwner)
                .WithMany() 
                .HasForeignKey(cr => cr.CarOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarRegistration>()
                .HasOne(cr => cr.CarModel)
                .WithMany() 
                .HasForeignKey(cr => cr.CarModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarEngineType>().HasData(
                new CarEngineType { Id = 1, Type = "Petrol", Abrv = "PET" },
                new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DSL" },
                new CarEngineType { Id = 3, Type = "Electric", Abrv = "ELE" },
                new CarEngineType { Id = 4, Type = "Hybrid", Abrv = "HYB" }
            );
        }
    }
}





