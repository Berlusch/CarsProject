using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;
using Microsoft.EntityFrameworkCore;

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

            query = query.Include(cm => cm.CarMake);
            query = query.Include(cm => cm.CarEngineType);

            if (pfs.Paging.PageSize > 0)
            {
                query = query.Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                             .Take(pfs.Paging.PageSize);
            }

            return await query.ToListAsync();
        }

      
        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            var query = _carModelRepository.GetQuery(new PSFParameters())
                                           .Include(cm => cm.CarMake)
                                           .Include(cm => cm.CarEngineType);

            var carModel = await query.FirstOrDefaultAsync(cm => cm.Id == id);

            if (carModel == null)
                throw new KeyNotFoundException($"CarModel with ID {id} not found.");

            return carModel;
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
            existing.CarMakeId = carModel.CarMakeId;          
            existing.CarEngineTypeId = carModel.CarEngineTypeId; 

            return await _carModelRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteCarModelAsync(int id)
        {
            return await _carModelRepository.DeleteAsync(id);
        }
    }
}