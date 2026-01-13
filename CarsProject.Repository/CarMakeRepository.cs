using CarsProject.DAL;
using CarsProject.WebApi;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarMakeRepository : GenericRepository<CarMake>, ICarMakeRepository
    {
        private readonly CarsDbContext _context;
        
        public CarMakeRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }
                
        public async Task<IEnumerable<CarMake>> GetAllCarMakesAsync()
        {
            return await _context.CarMakes.ToListAsync(); 
        }
    }
}


