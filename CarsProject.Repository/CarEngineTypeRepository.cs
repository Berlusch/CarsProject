using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarEngineTypeRepository : GenericRepository<CarEngineType>, ICarEngineTypeRepository
    {
        private readonly CarsDbContext _context;

        // Konstruktori
        public CarEngineTypeRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementacija metode za preuzimanje svih CarEngineType podataka
        public async Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync()
        {
            return await _context.CarEngineTypes.ToListAsync(); // Dohvati sve CarEngineTypes iz baze   
        }
    }
}
