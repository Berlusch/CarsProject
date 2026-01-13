using AutoMapper;
using CarsProject.WebApi;
using CarsProject.WebApi.DTO;
using CarsProject.Repository.Common;

namespace CarsProject.Service
{
    public class CarModelService : ICarModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
                
        public async Task<IEnumerable<CarModelDTORead>> GetCarModelsPagedAsync(int pageNumber, int pageSize, string sortBy, string filter)
        {
            var carModelsQuery = await _unitOfWork.CarModelRepository.GetAllCarModelsAsync();

            
            if (!string.IsNullOrEmpty(filter))
            {
                string lowerFilter = filter.ToLower();
                carModelsQuery = carModelsQuery.Where(c => c.Name.ToLower().Contains(lowerFilter));
            }

            
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "name")
                {
                    carModelsQuery = carModelsQuery.OrderBy(c => c.Name);
                }
                else
                {
                    carModelsQuery = carModelsQuery.OrderBy(c => c.Id);
                }
            }
                        
            var carModelsPaged = carModelsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();  

            
            var result = _mapper.Map<List<CarModelDTORead>>(carModelsPaged);

            return result;
        }


        public async Task<CarModelDTORead> GetCarModelByIdAsync(int id)
        {
            var carModel = await _unitOfWork.CarModelRepository.GetByIdAsync(id);

            if (carModel == null)
            {
                throw new Exception($"CarModel with ID {id} not found.");
            }

            return _mapper.Map<CarModelDTORead>(carModel);
        }

        public async Task<CarModelDTORead> AddCarModelAsync(CarModelDTOInsertUpdate carModelDto)
        {            
            var carMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(carModelDto.CarMakeId);
            if (carMake == null)
            {
                throw new Exception($"CarMake with ID {carModelDto.CarMakeId} not found.");
            }
            
            var carEngineType = await _unitOfWork.CarEngineTypeRepository.GetByIdAsync(carModelDto.CarEngineTypeId);
            if (carEngineType == null)
            {
                throw new Exception($"CarEngineType with ID {carModelDto.CarEngineTypeId} not found.");
            }
                        
            var carModels = await _unitOfWork.CarModelRepository.GetAllCarModelsAsync();
            var existingCarModel = carModels.FirstOrDefault(c => c.Name.Equals(carModelDto.Name, StringComparison.OrdinalIgnoreCase));


            if (existingCarModel != null)
            {
                throw new Exception($"CarModel with the name {carModelDto.Name} already exists.");
            }

            var carModel = _mapper.Map<CarModel>(carModelDto);
            carModel.CarMake = carMake;
            carModel.CarEngineType = carEngineType;


            var addedCarModel = await _unitOfWork.CarModelRepository.AddAsync(carModel);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarModelDTORead>(addedCarModel);
        }

        public async Task<CarModelDTORead> UpdateCarModelAsync(int id, CarModelDTOInsertUpdate carModelDto)
        {
            var existingCarModel = await _unitOfWork.CarModelRepository.GetByIdAsync(id);

            if (existingCarModel == null)
            {
                throw new KeyNotFoundException($"CarModel with ID {id} not found.");
            }
                        
            var carMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(carModelDto.CarMakeId);
            if (carMake == null)
            {
                throw new Exception($"CarMake with ID {carModelDto.CarMakeId} not found.");
            }

            
            var carEngineType = await _unitOfWork.CarEngineTypeRepository.GetByIdAsync(carModelDto.CarEngineTypeId);
            if (carEngineType == null)
            {
                throw new Exception($"CarEngineType with ID {carModelDto.CarEngineTypeId} not found.");
            }

            
            _mapper.Map(carModelDto, existingCarModel);
            existingCarModel.CarMake = carMake;  
            existingCarModel.CarEngineType = carEngineType;  

            
            var updatedCarModel = await _unitOfWork.CarModelRepository.UpdateAsync(existingCarModel);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarModelDTORead>(updatedCarModel);
        }

        public async Task<bool> DeleteCarModelAsync(int id)
        {
            var existingCarModel = await _unitOfWork.CarModelRepository.GetByIdAsync(id);
            if (existingCarModel == null)
            {
                return false;
            }

            var deleted = await _unitOfWork.CarModelRepository.DeleteAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
