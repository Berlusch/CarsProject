using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

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

