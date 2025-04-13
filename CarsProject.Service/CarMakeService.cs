using AutoMapper;
using CarsProject.Model;
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

        
        public async Task<IEnumerable<CarMake>> GetCarMakesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter)
        {
            var carMakesQuery = await _unitOfWork.CarMakeRepository.GetAllCarMakesAsync(); 

            // Filtering
            if (!string.IsNullOrEmpty(filter))
            {
                carMakesQuery = carMakesQuery.Where(c => c.Name.Contains(filter)); // Filtering by name
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "name")
                {
                    carMakesQuery = carMakesQuery.OrderBy(c => c.Name);
                } // Sorting by name
                else
                {
                    carMakesQuery = carMakesQuery.OrderBy(c => c.Id); // Otherwise, sorting by ID
                }
            }

            // Pagination
            var carMakesPaged = carMakesQuery
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize) 
                .ToList(); 

            return carMakesPaged; 
        }

        
        public async Task<CarMake> GetCarMakeByIdAsync(int id)
        {
            var carMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (carMake == null)
            {
                throw new Exception($"CarMake with ID {id} not found.");
            }

            return carMake; 
        }

        
        public async Task<CarMake> AddCarMakeAsync(CarMake carMake)
        {
            var addedCarMake = await _unitOfWork.CarMakeRepository.AddAsync(carMake);
            await _unitOfWork.SaveChangesAsync();
            return addedCarMake; 
        }

        
        public async Task<CarMake> UpdateCarMakeAsync(int id, CarMake carMake)
        {
            var existingCarMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (existingCarMake == null)
            {
                throw new KeyNotFoundException($"CarMake with ID {id} not found.");
            }

            carMake.Id = id; 

            var updatedCarMake = await _unitOfWork.CarMakeRepository.UpdateAsync(carMake);
            await _unitOfWork.SaveChangesAsync();

            return updatedCarMake; 
        }

        
        public async Task<bool> DeleteCarMakeAsync(int id)
        {
            var deleted = await _unitOfWork.CarMakeRepository.DeleteAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
