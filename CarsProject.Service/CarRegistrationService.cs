using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Model.Common;
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

        public async Task<IEnumerable<CarRegistration>> GetCarRegistrationsAsync(PFSParameters pfs)
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
            var query = _carRegistrationRepository.GetQuery(new PFSParameters())
                                           .Include(cm => cm.CarModel)
                                           .Include(cm => cm.CarOwner);

            var carModel = await query.FirstOrDefaultAsync(cm => cm.Id == id);

            if (carModel == null)
                throw new KeyNotFoundException($"CarModel with ID {id} not found.");

            return carModel;
        }

        public async Task<CarRegistration> AddCarRegistrationAsync(CarRegistration carRegistration)
        {            
            var existing = await _carRegistrationRepository.GetQuery(new PFSParameters())
                .FirstOrDefaultAsync(c => c.RegistrationNumber.ToLower() == carRegistration.RegistrationNumber.ToLower());

            if (existing != null)
                throw new Exception($"CarRegistration with number {carRegistration.RegistrationNumber} already exists.");
            
            var added = await _carRegistrationRepository.AddAsync(carRegistration);
            
            var result = await _carRegistrationRepository.GetQuery(new PFSParameters())
                                                          .Include(cr => cr.CarModel)
                                                          .Include(cr => cr.CarOwner)
                                                          .FirstOrDefaultAsync(cr => cr.Id == added.Id);

            return result!;
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
