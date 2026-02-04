using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;
using CarsProject.Common;

namespace CarsProject.Service
{
    public class CarMakeService(IGenericRepository<CarMake> _carMakeRepository) : ICarMakeService
    {
        public async Task<IEnumerable<CarMake>> GetCarMakesAsync(PFSParameters pfs)
        {
            var query = _carMakeRepository.GetQuery(pfs);

            if (pfs.Paging.PageSize > 0)
                query = query.Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                             .Take(pfs.Paging.PageSize);

            return await Task.FromResult(query.ToList());
        }

        public async Task<CarMake> GetCarMakeByIdAsync(int id)
        {
            var carMake = await _carMakeRepository.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException($"CarMake with ID {id} not found.");

            return carMake;
        }

        public async Task<CarMake> AddCarMakeAsync(CarMake carMake)
        {
            ArgumentNullException.ThrowIfNull(carMake);

            var existing = _carMakeRepository.GetQuery(new PFSParameters
            {
                Filter = new FilterParameters { PropertyName = "Name", Filter = carMake.Name }
            }).FirstOrDefault();

            if (existing != null)
                throw new Exception($"CarMake with the name {carMake.Name} already exists.");

            return await _carMakeRepository.AddAsync(carMake);
        }

        public async Task<CarMake> UpdateCarMakeAsync(int id, CarMake carMake)
        {
            ArgumentNullException.ThrowIfNull(carMake);

            var existing = await _carMakeRepository.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException($"CarMake with ID {id} not found.");

            existing.Name = carMake.Name;
            existing.Abrv = carMake.Abrv;

            return await _carMakeRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteCarMakeAsync(int id)
        {
            return await _carMakeRepository.DeleteAsync(id);
        }
    }
}