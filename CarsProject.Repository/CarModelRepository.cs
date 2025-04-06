using CarsProject.DAL;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    
    public class CarModelRepository : GenericRepository<CarsProject.Model.CarModel>, ICarModelRepository
    {
        public CarModelRepository(CarsDbContext context) : base(context)
        {
            //specific methods will be applied here later
        }

        
    }
}

