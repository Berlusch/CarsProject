using CarsProject.Common;
using CarsProject.Common.QueryableExtensions;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Service
{
    public class CarRegistrationService(
        IGenericRepository<CarRegistration> _carRegistrationRepository,
        IGenericRepository<CarOwner> _carOwnerRepository,
        IGenericRepository<CarModel> _carModelRepository
    ) : ICarRegistrationService
    {
        public async Task<IEnumerable<CarRegistration>> GetCarRegistrationsAsync(PFSParameters pfs)
        {
            var query = _carRegistrationRepository.GetQuery(pfs)
                                                  .IncludeFKs();

            if (pfs.Paging.PageSize > 0)
            {
                query = query.Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                             .Take(pfs.Paging.PageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<CarRegistration> GetCarRegistrationByIdAsync(int id)
        {
            var carRegistration = await _carRegistrationRepository.GetQuery(new PFSParameters())
                                                                 .IncludeFKs()
                                                                 .FirstOrDefaultAsync(cr => cr.Id == id)
                               ?? throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");

            return carRegistration;
        }

        public async Task<CarRegistration> AddCarRegistrationAsync(CarRegistration carRegistration)
        {
            ArgumentNullException.ThrowIfNull(carRegistration);

            var existing = await _carRegistrationRepository.GetQuery(new PFSParameters())
                .FirstOrDefaultAsync(c => c.RegistrationNumber.ToLower() == carRegistration.RegistrationNumber.ToLower());

            if (existing != null)
                throw new Exception($"CarRegistration with number {carRegistration.RegistrationNumber} already exists.");

            var added = await _carRegistrationRepository.AddAsync(carRegistration);

            var result = await _carRegistrationRepository.GetQuery(new PFSParameters())
                                                          .IncludeFKs()
                                                          .FirstOrDefaultAsync(cr => cr.Id == added.Id)
                         ?? throw new KeyNotFoundException($"CarRegistration with ID {added.Id} not found after insert.");

            return result;
        }

        public async Task<CarRegistration> UpdateCarRegistrationAsync(int id, CarRegistration carRegistration)
        {
            ArgumentNullException.ThrowIfNull(carRegistration);

            var existing = await _carRegistrationRepository.GetByIdAsync(id)
                            ?? throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");

            var carOwner = await _carOwnerRepository.GetByIdAsync(carRegistration.CarOwnerId)
                           ?? throw new Exception($"CarOwner with ID {carRegistration.CarOwnerId} not found.");

            var carModel = await _carModelRepository.GetByIdAsync(carRegistration.CarModelId)
                           ?? throw new Exception($"CarModel with ID {carRegistration.CarModelId} not found.");

            existing.RegistrationNumber = carRegistration.RegistrationNumber;
            existing.CarOwner = carOwner;
            existing.CarModel = carModel;
            existing.CarOwnerId = carRegistration.CarOwnerId;
            existing.CarModelId = carRegistration.CarModelId;

            await _carRegistrationRepository.UpdateAsync(existing);

            var result = await _carRegistrationRepository.GetQuery(new PFSParameters())
                                                          .IncludeFKs()
                                                          .FirstOrDefaultAsync(cr => cr.Id == existing.Id)
                         ?? throw new KeyNotFoundException($"CarRegistration with ID {existing.Id} not found after update.");

            return result;
        }

        public async Task<bool> DeleteCarRegistrationAsync(int id)
        {
            return await _carRegistrationRepository.DeleteAsync(id);
        }
    }
}