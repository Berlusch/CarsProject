using CarsProject.DAL;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class CarMakeRepository : GenericRepository<CarsProject.Model.CarMake>, ICarMakeRepository
    {
        public CarMakeRepository(CarsDbContext context) : base(context)
        {
            //specific methods will be applied here later
        }


    }
}

