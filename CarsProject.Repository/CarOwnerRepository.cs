using CarsProject.DAL;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class CarOwnerRepository : GenericRepository<CarsProject.Model.CarOwner>, ICarOwnerRepository
    {
        public CarOwnerRepository(CarsDbContext context) : base(context)
        {
            //specific methods will be applied here later
        }


    }
}

