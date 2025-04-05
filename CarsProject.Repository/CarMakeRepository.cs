using CarsProject.Model;  
using CarsProject.Repository.Common;  
using CarsProject.DAL;  
using Microsoft.EntityFrameworkCore;  

namespace CarsProject.Repository
{
    public class CarMakeRepository : ICarMakeRepository
    {
        private readonly CarsDbContext _context;

        public CarMakeRepository(CarsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarsProject.Model.CarMake>> GetAllAsync()
        {
            return await _context.CarMakes.ToListAsync();  
        }

        public async Task<CarsProject.Model.CarMake> GetByIdAsync(Guid id)
        {
            {
                var carMake = await _context.CarMakes.FindAsync(id);
                if (carMake == null)
                {
                    throw new KeyNotFoundException($"CarMake with ID {id} not found.");
                }
                return carMake;
            }
        }
        
        public async Task AddAsync(CarsProject.Model.CarMake carMake)
        {
            _context.CarMakes.Add(carMake);  
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarsProject.Model.CarMake carMake)
        {
            _context.CarMakes.Update(carMake);  
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var carMake = await _context.CarMakes.FindAsync(id);
            if (carMake != null)
            {
                _context.CarMakes.Remove(carMake);  
                await _context.SaveChangesAsync();
            }
        }
    }
}

