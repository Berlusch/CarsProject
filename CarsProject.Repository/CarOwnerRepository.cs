using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarOwnerRepository : GenericRepository<CarOwner>, ICarOwnerRepository
    {
        private readonly CarsDbContext _context;
       
        public CarOwnerRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<CarOwner>> GetAllCarOwnersAsync()
        {
            return await _context.CarOwners.ToListAsync(); 
        }
    }


    
}

