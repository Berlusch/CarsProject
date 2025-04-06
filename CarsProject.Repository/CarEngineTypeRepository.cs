using CarsProject.DAL;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarEngineTypeRepository : ICarEngineTypeRepository
    {
        private readonly CarsDbContext _context;

        public CarEngineTypeRepository(CarsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarsProject.Model.CarEngineType>> GetAllAsync()
        {
            return await _context.CarEngineTypes.ToListAsync();
        }

        public async Task<CarsProject.Model.CarEngineType> GetByIdAsync(Guid id)
        {
            var engineType = await _context.CarEngineTypes.FindAsync(id);
            if (engineType == null)
            {
                throw new KeyNotFoundException($"CarEngineType with ID {id} not found.");
            }
            return engineType;
        }
    }
}
