using CarsProject.Model;  
using CarsProject.Repository.Common;  
using CarsProject.DAL;  
using Microsoft.EntityFrameworkCore;  

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

