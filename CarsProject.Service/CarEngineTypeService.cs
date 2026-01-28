using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;
using CarsProject.Common;

namespace CarsProject.Service
{
    public class CarEngineTypeService : ICarEngineTypeService
    {
        private readonly ICarEngineTypeRepository _carEngineTypeRepository;

        public CarEngineTypeService(ICarEngineTypeRepository carEngineTypeRepository)
        {
            _carEngineTypeRepository = carEngineTypeRepository;
        }

        public async Task<IEnumerable<CarEngineType>> GetCarEngineTypesAsync(PSFParameters pfs)
        {            
            pfs ??= new PSFParameters();
            pfs.Paging ??= new PagingParameters();
            pfs.Sorting ??= new SortingParameters();
            pfs.Filter ??= new FilterParameters();
            
            if (pfs.Paging.PageNumber <= 0)
                pfs.Paging.PageNumber = 1;

            if (pfs.Paging.PageSize <= 0)
                pfs.Paging.PageSize = 1000;
            
            if (string.IsNullOrEmpty(pfs.Sorting.OrderBy))
                pfs.Sorting.OrderBy = "Type";

            return await _carEngineTypeRepository.GetAllCarEngineTypesAsync(pfs);
        }

        public async Task<CarEngineType> GetCarEngineTypeByIdAsync(int id)
        {
            var carEngineType = await _carEngineTypeRepository.GetByIdAsync(id);

            if (carEngineType == null)
                throw new Exception($"CarEngineType with ID {id} not found.");

            return carEngineType;
        }
    }
}
