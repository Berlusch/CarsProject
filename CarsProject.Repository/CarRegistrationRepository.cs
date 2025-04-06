using CarsProject.DAL;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class CarRegistrationRepository : GenericRepository<CarsProject.Model.CarRegistration>, ICarRegistrationRepository
    {
        public CarRegistrationRepository(CarsDbContext context) : base(context)
        {
            //specific methods will be applied here later
        }


    }
}

