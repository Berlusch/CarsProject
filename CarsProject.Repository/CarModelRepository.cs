using CarsProject.Common;
using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarModelRepository(CarsDbContext context)
    : GenericRepository<CarModel>(context), ICarModelRepository
    {   

        public async Task<PagedResult<CarModel>> GetPagedAsync(PFSParameters? parameters = null)
        {
            parameters ??= new PFSParameters();
            parameters.Paging ??= new PagingParameters();
       
            parameters.Paging.PageSize = parameters.Paging.PageSize != 0 ? parameters.Paging.PageSize : 5;

            var query = GetQuery(parameters);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(parameters.Paging.Skip)
                .Take(parameters.Paging.PageSize)
                .ToListAsync();

            return new PagedResult<CarModel>
            {
                Items = items,
                TotalCount = totalCount,
                Paging = parameters.Paging
            };
        }
    }
}