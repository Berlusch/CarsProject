using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;

namespace CarsProject.Service
{
    public class CarEngineTypeService(ICarEngineTypeRepository _carEngineTypeRepository) : ICarEngineTypeService
    {
        public async Task<PagedResult<CarEngineType>> GetCarEngineTypesAsync(PFSParameters pfs)
        {
            var query = _carEngineTypeRepository.GetQuery(pfs);

            var totalCount = query.Count();

            if (pfs.Paging.PageSize > 0)
            {
                query = query
                    .Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                    .Take(pfs.Paging.PageSize);
            }

            var items = query.ToList();

            return new PagedResult<CarEngineType>
            {
                Items = items,
                TotalCount = totalCount,
                Paging = pfs.Paging
            };
        }

        public async Task<CarEngineType> GetCarEngineTypeByIdAsync(int id)
        {
            var carEngineType = await _carEngineTypeRepository.GetByIdAsync(id)
                                ?? throw new KeyNotFoundException($"CarEngineType with ID {id} not found.");

            return carEngineType;
        }
    }
}