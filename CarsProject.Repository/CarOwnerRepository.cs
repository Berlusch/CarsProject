using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarOwnerRepository : ICarOwnerRepository
    {
        private readonly CarsDbContext _context;

        public CarOwnerRepository(CarsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarsProject.Model.CarOwner>> GetAllAsync()
        {
            return await _context.CarOwners.ToListAsync();
        }

        public async Task<CarsProject.Model.CarOwner> GetByIdAsync(Guid id)
        {
            var owner = await _context.CarOwners.FindAsync(id);
            if (owner == null)
            {
                throw new KeyNotFoundException($"CarOwner with ID {id} not found.");
            }
            return owner;
        }

        public async Task AddAsync(CarsProject.Model.CarOwner owner)
        {
            _context.CarOwners.Add(owner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarsProject.Model.CarOwner owner)
        {
            _context.CarOwners.Update(owner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var owner = await _context.CarOwners.FindAsync(id);
            if (owner != null)
            {
                _context.CarOwners.Remove(owner);
                await _context.SaveChangesAsync();
            }
        }
    }
}

