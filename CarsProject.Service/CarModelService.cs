using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;

namespace CarsProject.Service
{
    public class CarModelService : ICarModelService
    {
        private readonly IGenericRepository<CarModel> _carModelRepository;

        public CarModelService(IGenericRepository<CarModel> carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        public async Task<IEnumerable<CarModel>> GetCarModelsAsync(PSFParameters pfs)
        {
            var query = _carModelRepository.GetQuery(pfs);

            if (pfs.Paging.PageSize > 0)
                query = query.Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                             .Take(pfs.Paging.PageSize);

            return await Task.FromResult(query.ToList());
        }

        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            return await _carModelRepository.GetByIdAsync(id);
        }

        public async Task<CarModel> AddCarModelAsync(CarModel carModel)
        {            
            var existing = _carModelRepository.GetQuery(new PSFParameters
            {
                Filter = new FilterParameters { PropertyName = "Name", Filter = carModel.Name }
            }).FirstOrDefault();

            if (existing != null)
                throw new Exception($"CarModel with the name {carModel.Name} already exists.");
            
            return await _carModelRepository.AddAsync(carModel);
        }

        public async Task<CarModel> UpdateCarModelAsync(int id, CarModel carModel)
        {
            var existing = await _carModelRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"CarModel with ID {id} not found.");
            
            existing.Name = carModel.Name;
            existing.Abrv = carModel.Abrv;
            existing.CarMake = carModel.CarMake;
            existing.CarEngineType = carModel.CarEngineType;

            return await _carModelRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteCarModelAsync(int id)
        {
            return await _carModelRepository.DeleteAsync(id);
        }
    }
}

