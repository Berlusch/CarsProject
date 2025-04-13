using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.Common;
using CarsProject.Model.DTO;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        // Paginacija, filtriranje i sortiranje
        public async Task<IEnumerable<CarMake>> GetCarMakesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter)
        {
            var carMakesQuery = await _unitOfWork.CarMakeRepository.GetAllCarMakesAsync();

            // Filtriranje
            if (!string.IsNullOrEmpty(filter))
            {
                carMakesQuery = carMakesQuery.Where(c => c.Name.Contains(filter));
            }

            // Sortiranje
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

            // Paginacija
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
            // Dohvati sve car makes iz baze i provjeri postoji li već entitet s istim nazivom
            var carMakes = await _unitOfWork.CarMakeRepository.GetAllCarMakesAsync(); // Ovo je IEnumerable<CarMake>

            var existingCarMake = carMakes.FirstOrDefault(c => c.Name.Equals(carMake.Name, StringComparison.OrdinalIgnoreCase));

            if (existingCarMake != null)
            {
                throw new Exception($"CarMake with the name {carMake.Name} already exists.");
            }

            var addedCarMake = await _unitOfWork.CarMakeRepository.AddAsync(carMake);
            await _unitOfWork.SaveChangesAsync();
            return addedCarMake;
        }


        public async Task<CarMakeDTORead> UpdateCarMakeAsync(int id, CarMakeDTOInsertUpdate carMakeDto)
        {
            var existingCarMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (existingCarMake == null)
            {
                throw new KeyNotFoundException($"CarMake with ID {id} not found.");
            }

            // Ovo ažurira existingCarMake pomoću podataka iz DTO-a
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
                return false; // Nema što za brisati
            }

            var deleted = await _unitOfWork.CarMakeRepository.DeleteAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
