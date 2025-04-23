using AutoMapper;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;

namespace CarsProject.Service
{
    public class CarEngineTypeService : ICarEngineTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarEngineTypeService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<CarEngineTypeDTORead>> GetCarEngineTypesPagedAsync
            (int pageNumber, int pageSize, string sortBy, string filter)
        {
            var carEngineTypesQuery = await _unitOfWork.CarEngineTypeRepository.GetAllCarEngineTypesAsync();

            
            if (!string.IsNullOrEmpty(filter))
            {
                string lowerFilter = filter.ToLower();
                carEngineTypesQuery = carEngineTypesQuery.Where(c => c.Type.ToLower().Contains(lowerFilter));
            }

            
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "type")
                {
                    carEngineTypesQuery = carEngineTypesQuery.OrderBy(c => c.Type);
                }
                else
                {
                    carEngineTypesQuery = carEngineTypesQuery.OrderBy(c => c.Id);
                }
            }

            
            var carEngineTypesPaged = carEngineTypesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            
            var result = _mapper.Map<IEnumerable<CarEngineTypeDTORead>>(carEngineTypesPaged);

            return result;
        }

        public async Task<CarEngineTypeDTORead> GetCarEngineTypeByIdAsync(int id)
        {
            var carEngineType = await _unitOfWork.CarEngineTypeRepository.GetByIdAsync(id);

            if (carEngineType == null)
            {
                throw new Exception($"CarEngineType with ID {id} not found.");
            }

            return _mapper.Map<CarEngineTypeDTORead>(carEngineType);
        }

        
    }
}

