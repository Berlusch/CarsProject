using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

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

