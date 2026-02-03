using CarsProject.Common;
using CarsProject.DAL;
using CarsProject.Repository.Common;
using CarsProject.Model;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarEngineTypeRepository : GenericRepository<CarEngineType>, ICarEngineTypeRepository
    {
        public CarEngineTypeRepository(CarsDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync(PFSParameters psf)
        {
            var query = GetQuery(psf);
            return await query
                .Skip((psf.Paging.PageNumber - 1) * psf.Paging.PageSize)
                .Take(psf.Paging.PageSize)
                .ToListAsync();
        }
    }
}
