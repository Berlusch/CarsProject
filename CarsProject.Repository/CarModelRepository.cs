using CarsProject.Common;
using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        public CarModelRepository(CarsDbContext context)
            : base(context)
        {
        }

        public override IQueryable<CarModel> GetQuery(PSFParameters parameters)
        {
            IQueryable<CarModel> query = _dbSet
                .Include(cm => cm.CarMake)
                .Include(cm => cm.CarEngineType);
            

            return query;
        }

        public override async Task<CarModel> GetByIdAsync(int id)
        {
            var carModel = await _dbSet
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





