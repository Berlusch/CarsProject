using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class CarMakeRepository : GenericRepository<CarMake>, ICarMakeRepository
    {
        public CarMakeRepository(CarsDbContext context)
            : base(context)
        {

        }

        
    }
}




