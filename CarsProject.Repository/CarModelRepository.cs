using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        public CarModelRepository(CarsDbContext context) : base(context)
        {
        }
        
    }
}