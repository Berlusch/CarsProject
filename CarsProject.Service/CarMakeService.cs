using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;

namespace CarsProject.Service
{
    public class CarMakeService : ICarMakeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarMakeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // PFS (pagination, filtering, sorting)
        public async Task<IEnumerable<CarMakeDTORead>> GetCarMakesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter)
        {
            var carMakesQuery = await _unitOfWork.CarMakeRepository.GetAllCarMakesAsync();

            // Filtering
            if (!string.IsNullOrEmpty(filter))
            {
                string lowerFilter = filter.ToLower();
                carMakesQuery = carMakesQuery.Where(c => c.Name.ToLower().Contains(lowerFilter));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "name")
                {
                    carMakesQuery = carMakesQuery.OrderBy(c => c.Name);
                }
                else
                {
                    carMakesQuery = carMakesQuery.OrderBy(c => c.Id);
                }
            }

            // Pagination
            var carMakesPaged = carMakesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            
            var result = _mapper.Map<IEnumerable<CarMakeDTORead>>(carMakesPaged);

            return result;
        }

        public async Task<CarMakeDTORead> GetCarMakeByIdAsync(int id)
        {
            var carMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (carMake == null)
            {
                throw new Exception($"CarMake with ID {id} not found.");
            }

            return _mapper.Map<CarMakeDTORead>(carMake);
        }

        public async Task<CarMakeDTORead> AddCarMakeAsync(CarMakeDTOInsertUpdate carMakeDto)
        {
            

            var carMakes = await _unitOfWork.CarMakeRepository.GetAllCarMakesAsync(); 


            var existingCarMake = carMakes.FirstOrDefault(c => c.Name.Equals(carMakeDto.Name, StringComparison.OrdinalIgnoreCase));

            if (existingCarMake != null)
            {
                throw new Exception($"CarMake with the name {carMakeDto.Name} already exists.");
            }

            

            var carMake = _mapper.Map<CarMake>(carMakeDto);

            var addedCarMake = await _unitOfWork.CarMakeRepository.AddAsync(carMake);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarMakeDTORead>(addedCarMake);
        }

        public async Task<CarMakeDTORead> UpdateCarMakeAsync(int id, CarMakeDTOInsertUpdate carMakeDto)
        {
            var existingCarMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (existingCarMake == null)
            {
                throw new KeyNotFoundException($"CarMake with ID {id} not found.");
            }

            

            _mapper.Map(carMakeDto, existingCarMake);

            var updatedCarMake = await _unitOfWork.CarMakeRepository.UpdateAsync(existingCarMake);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarMakeDTORead>(updatedCarMake);
        }

        public async Task<bool> DeleteCarMakeAsync(int id)
        {
            var existingCarMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);
            if (existingCarMake == null)
            {
                return false; 

            }

            var deleted = await _unitOfWork.CarMakeRepository.DeleteAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
