using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarOwnerRepository : GenericRepository<CarOwner>, ICarOwnerRepository
    {
        private readonly CarsDbContext _context;

        // Konstruktori
        public CarOwnerRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementacija metode za preuzimanje svih CarOwners podataka
        public async Task<IEnumerable<CarOwner>> GetAllCarOwnersAsync()
        {
            return await _context.CarOwners.ToListAsync(); // Dohvati sve CarOwners iz baze
        }
    }


    
}

