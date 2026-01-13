using CarsProject.DAL;
using CarsProject.WebApi;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarRegistrationRepository : GenericRepository<CarRegistration>, ICarRegistrationRepository
    {
        private readonly CarsDbContext _context;
        
        public CarRegistrationRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarRegistration>> GetAllCarRegistrationsAsync()
        {

            return await _context.CarRegistrations
                .Include(cm => cm.CarOwner)
                .Include(cm => cm.CarModel)
                .ToListAsync();
        }

        public override async Task<CarRegistration> GetByIdAsync(int id)
        {

            var carRegistration = await _context.CarRegistrations
                .Include(cm => cm.CarOwner)
                .Include(cm => cm.CarModel)
                .FirstOrDefaultAsync(cm => cm.Id == id);

            if (carRegistration == null)
            {
                throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");
            }

            return carRegistration;
        }
    }
}






