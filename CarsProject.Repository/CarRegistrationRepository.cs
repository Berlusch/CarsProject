using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarRegistrationRepository : GenericRepository<CarRegistration>, ICarRegistrationRepository
    {
        public CarRegistrationRepository(CarsDbContext context)
            : base(context)
        {
        }
        
        public override async Task<CarRegistration> GetByIdAsync(int id)
        {
            var carRegistration = await _dbSet
                .Include(cr => cr.CarOwner)
                .Include(cr => cr.CarModel)
                .FirstOrDefaultAsync(cr => cr.Id == id);

            if (carRegistration == null)
            {
                throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");
            }

            return carRegistration;
        }
    }
}







