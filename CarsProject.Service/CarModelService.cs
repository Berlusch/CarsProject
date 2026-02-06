using CarsProject.Common;
using CarsProject.Common.QueryableExtensions;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Service
{
    public class CarModelService(IGenericRepository<CarModel> _carModelRepository) : ICarModelService
    {
        public async Task<PagedResult<CarModel>> GetCarModelsAsync(PFSParameters pfs)
        {
            var query = _carModelRepository.GetQuery(pfs)
                                            .IncludeFKs();

            var totalCount = query.Count();

            if (pfs.Paging.PageSize > 0)
            {
                query = query
                    .Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                    .Take(pfs.Paging.PageSize);
            }

            var items = query.ToList();

            return new PagedResult<CarModel>
            {
                Items = items,
                TotalCount = totalCount,
                Paging = pfs.Paging
            };
        }

        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            var carModel = await _carModelRepository.GetQuery(new PFSParameters())
                                                    .IncludeFKs()
                                                    .FirstOrDefaultAsync(cm => cm.Id == id)
                            ?? throw new KeyNotFoundException($"CarModel with ID {id} not found.");

            return carModel;
        }

        public async Task<CarModel> AddCarModelAsync(CarModel carModel)
        {
            ArgumentNullException.ThrowIfNull(carModel);

            var existing = _carModelRepository.GetQuery(new PFSParameters
            {
                Filter = new FilterParameters { PropertyName = "Name", Filter = carModel.Name }
            }).FirstOrDefault();

            if (existing != null)
                throw new Exception($"CarModel with the name {carModel.Name} already exists.");

            var added = await _carModelRepository.AddAsync(carModel);

            var result = await _carModelRepository.GetQuery(new PFSParameters())
                                                  .IncludeFKs()
                                                  .FirstOrDefaultAsync(cm => cm.Id == added.Id)
                         ?? throw new KeyNotFoundException($"CarModel with ID {added.Id} not found after insert.");

            return result;
        }

        public async Task<CarModel> UpdateCarModelAsync(int id, CarModel carModel)
        {
            ArgumentNullException.ThrowIfNull(carModel);

            var existing = await _carModelRepository.GetByIdAsync(id)
                            ?? throw new KeyNotFoundException($"CarModel with ID {id} not found.");

            existing.Name = carModel.Name;
            existing.Abrv = carModel.Abrv;
            existing.CarMakeId = carModel.CarMakeId;
            existing.CarEngineTypeId = carModel.CarEngineTypeId;

            await _carModelRepository.UpdateAsync(existing);

            var result = await _carModelRepository.GetQuery(new PFSParameters())
                                                  .IncludeFKs()
                                                  .FirstOrDefaultAsync(cm => cm.Id == existing.Id)
                         ?? throw new KeyNotFoundException($"CarModel with ID {existing.Id} not found after update.");

            return result;
        }

        public async Task<bool> DeleteCarModelAsync(int id)
        {
            return await _carModelRepository.DeleteAsync(id);
        }
    }
}