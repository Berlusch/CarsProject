using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarRegistrationRepository : GenericRepository<CarRegistration>, ICarRegistrationRepository
    {
        public CarRegistrationRepository(CarsDbContext context)
            : base(context)
        {
        }
        
        
    }
}







