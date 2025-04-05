using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly CarsDbContext _context;

        public CarModelRepository(CarsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarsProject.Model.CarModel>> GetAllAsync()
        {
            return await _context.CarModels.ToListAsync();
        }

        public async Task<CarsProject.Model.CarModel> GetByIdAsync(Guid id)
        {
            {
                var carModel = await _context.CarModels.FindAsync(id);
                if (carModel == null)
                {
                    throw new KeyNotFoundException($"CarModel with ID {id} not found.");
                }
                return carModel;
            }
        }

        public async Task AddAsync(CarsProject.Model.CarModel carModel)
        {
            _context.CarModels.Add(carModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarsProject.Model.CarModel carModel)
        {
            _context.CarModels.Update(carModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var carModel = await _context.CarModels.FindAsync(id);
            if (carModel != null)
            {
                _context.CarModels.Remove(carModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}

