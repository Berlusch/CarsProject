using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

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

