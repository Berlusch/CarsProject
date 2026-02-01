using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Service
{
    public class CarRegistrationService : ICarRegistrationService
    {
        private readonly IGenericRepository<CarRegistration> _carRegistrationRepository;
        private readonly IGenericRepository<CarOwner> _carOwnerRepository;
        private readonly IGenericRepository<CarModel> _carModelRepository;

        public CarRegistrationService(
            IGenericRepository<CarRegistration> carRegistrationRepository,
            IGenericRepository<CarOwner> carOwnerRepository,
            IGenericRepository<CarModel> carModelRepository)
        {
            _carRegistrationRepository = carRegistrationRepository;
            _carOwnerRepository = carOwnerRepository;
            _carModelRepository = carModelRepository;
        }

        public async Task<IEnumerable<CarRegistration>> GetCarRegistrationsAsync(PSFParameters pfs)
        {
            var query = _carRegistrationRepository.GetQuery(pfs);

            query = query.Include(cm => cm.CarModel);
            query = query.Include(cm => cm.CarOwner);

            if (pfs.Paging.PageSize > 0)
            {
                query = query.Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                             .Take(pfs.Paging.PageSize);
            }

            return await query.ToListAsync();
        }


        public async Task<CarRegistration> GetCarRegistrationByIdAsync(int id)
        {
            var query = _carRegistrationRepository.GetQuery(new PSFParameters())
                                           .Include(cm => cm.CarModel)
                                           .Include(cm => cm.CarOwner);

            var carModel = await query.FirstOrDefaultAsync(cm => cm.Id == id);

            if (carModel == null)
                throw new KeyNotFoundException($"CarModel with ID {id} not found.");

            return carModel;
        }

        public async Task<CarRegistration> AddCarRegistrationAsync(CarRegistration carRegistration)
        {            
            var carOwner = await _carOwnerRepository.GetByIdAsync(carRegistration.CarOwnerId);
            if (carOwner == null)
                throw new System.Exception($"CarOwner with ID {carRegistration.CarOwnerId} not found.");
            
            var carModel = await _carModelRepository.GetByIdAsync(carRegistration.CarModelId);
            if (carModel == null)
                throw new System.Exception($"CarModel with ID {carRegistration.CarModelId} not found.");
            
            var existing = _carRegistrationRepository.GetQuery(new PSFParameters
            {
                Filter = new FilterParameters { PropertyName = "RegistrationNumber", Filter = carRegistration.RegistrationNumber }
            }).FirstOrDefault(c => c.RegistrationNumber.Equals(carRegistration.RegistrationNumber, System.StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                throw new System.Exception($"CarRegistration with RegistrationNumber {carRegistration.RegistrationNumber} already exists.");

            carRegistration.CarOwner = carOwner;
            carRegistration.CarModel = carModel;

            return await _carRegistrationRepository.AddAsync(carRegistration);
        }

        public async Task<CarRegistration> UpdateCarRegistrationAsync(int id, CarRegistration carRegistration)
        {
            var existing = await _carRegistrationRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");
            
            var carOwner = await _carOwnerRepository.GetByIdAsync(carRegistration.CarOwnerId);
            if (carOwner == null)
                throw new System.Exception($"CarOwner with ID {carRegistration.CarOwnerId} not found.");
            
            var carModel = await _carModelRepository.GetByIdAsync(carRegistration.CarModelId);
            if (carModel == null)
                throw new System.Exception($"CarModel with ID {carRegistration.CarModelId} not found.");
            
            existing.RegistrationNumber = carRegistration.RegistrationNumber;
            existing.CarOwner = carOwner;
            existing.CarModel = carModel;
            existing.CarOwnerId = carRegistration.CarOwnerId;
            existing.CarModelId = carRegistration.CarModelId;

            return await _carRegistrationRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteCarRegistrationAsync(int id)
        {
            return await _carRegistrationRepository.DeleteAsync(id);
        }
    }
}
