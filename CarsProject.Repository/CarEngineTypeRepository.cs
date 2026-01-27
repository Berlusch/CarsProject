using CarsProject.Common;
using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.WebApi;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarEngineTypeRepository : GenericRepository<CarEngineType>, ICarEngineTypeRepository
    {
        public CarEngineTypeRepository(CarsDbContext context)
            : base(context) 
        {
        }

        public async Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync(PSFParameters psf)
        {
            var query = GetQuery(psf); 
            return await query
                .Skip((psf.Paging.PageNumber - 1) * psf.Paging.PageSize)
                .Take(psf.Paging.PageSize)
                .ToListAsync();
        }
    }
}
