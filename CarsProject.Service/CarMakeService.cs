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

        public async Task<IEnumerable<CarMakeDTORead>> GetAllCarMakesAsync()
        {
            var carMakes = await _unitOfWork.CarMakeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CarMakeDTORead>>(carMakes);
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
            var carMake = _mapper.Map<CarMake>(carMakeDto);
            var added = await _unitOfWork.CarMakeRepository.AddAsync(carMake);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CarMakeDTORead>(added);
        }

        public async Task<CarMakeDTORead> UpdateCarMakeAsync(int id, CarMakeDTOInsertUpdate carMakeDto)
        {
            var existingCarMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (existingCarMake == null)
            {
                
                throw new KeyNotFoundException($"CarMake with ID {id} not found.");
            }

            
            var carMakeToUpdate = _mapper.Map<CarMake>(carMakeDto);
            carMakeToUpdate.Id = id; // Postavljamo ID kako bi se ažurirao pravi entitet

            
            var updatedCarMake = await _unitOfWork.CarMakeRepository.UpdateAsync(carMakeToUpdate);
            await _unitOfWork.SaveChangesAsync();

            
            return _mapper.Map<CarMakeDTORead>(updatedCarMake);
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
