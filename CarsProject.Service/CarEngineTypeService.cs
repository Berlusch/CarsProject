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
            if (pfs.Paging.PageSize <= 0)
            {
                pfs.Paging.PageNumber = 1;
                pfs.Paging.PageSize = 1000;
            }

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

