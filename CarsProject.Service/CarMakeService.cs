using CarsProject.Model;
using CarsProject.Repository.Common;


namespace CarsProject.Service
{
    public class CarMakeService : ICarMakeService
    {
        private readonly ICarMakeRepository _carMakeRepository;

        
        public CarMakeService(ICarMakeRepository carMakeRepository)
        {
            _carMakeRepository = carMakeRepository;
        }

       
        public async Task<IEnumerable<CarMake>> GetAllCarMakesAsync()
        {
            return await _carMakeRepository.GetAllAsync();
        }

        
        public async Task<CarMake> GetCarMakeByIdAsync(int id)
        {
            return await _carMakeRepository.GetByIdAsync(id);
        }

       
        public async Task<CarMake> AddCarMakeAsync(CarMake carMake)
        {
            return await _carMakeRepository.AddAsync(carMake);
        }

       
        public async Task<CarMake> UpdateCarMakeAsync(CarMake carMake)
        {
            return await _carMakeRepository.UpdateAsync(carMake);
        }

        
        public async Task<bool> DeleteCarMakeAsync(int id)
        {
            return await _carMakeRepository.DeleteAsync(id);
        }
    }
}

