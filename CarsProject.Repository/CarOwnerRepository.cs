using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class CarOwnerRepository : GenericRepository<CarOwner>, ICarOwnerRepository
    {
        public CarOwnerRepository(CarsDbContext context)
            : base(context)
        {
        }


    }
}


