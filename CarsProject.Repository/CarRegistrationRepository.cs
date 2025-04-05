using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarRegistrationRepository : ICarRegistrationRepository
    {
        private readonly CarsDbContext _context;

        public CarRegistrationRepository(CarsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarsProject.Model.CarRegistration>> GetAllAsync()
        {
            return await _context.CarRegistrations.ToListAsync();
        }

        public async Task<CarsProject.Model.CarRegistration> GetByIdAsync(Guid id)
        {
            var registration = await _context.CarRegistrations.FindAsync(id);
            if (registration == null)
            {
                throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");
            }
            return registration;
        }

        public async Task AddAsync(CarsProject.Model.CarRegistration registration)
        {
            _context.CarRegistrations.Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarsProject.Model.CarRegistration registration)
        {
            _context.CarRegistrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var registration = await _context.CarRegistrations.FindAsync(id);
            if (registration != null)
            {
                _context.CarRegistrations.Remove(registration);
                await _context.SaveChangesAsync();
            }
        }
    }
}

