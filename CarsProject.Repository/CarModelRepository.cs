using CarsProject.DAL;
using CarsProject.WebApi;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        private readonly CarsDbContext _context;

        
        public CarModelRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }
                
        public async Task<IEnumerable<CarModel>> GetAllCarModelsAsync()
        {
            
            return await _context.CarModels
                .Include(cm => cm.CarMake)       
                .Include(cm => cm.CarEngineType) 
                .ToListAsync();
        }

        public override async Task<CarModel> GetByIdAsync(int id)
        {
            
            var carModel = await _context.CarModels
                .Include(cm => cm.CarMake)       
                .Include(cm => cm.CarEngineType) 
                .FirstOrDefaultAsync(cm => cm.Id == id);  

            if (carModel == null)
            {
                throw new KeyNotFoundException($"CarModel with ID {id} not found.");
            }

            return carModel;
        }
    }
}




