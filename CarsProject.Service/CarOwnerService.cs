using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository;
using CarsProject.Repository.Common;
using CarsProject.Service.Common;


namespace CarsProject.Service
{
    public class CarOwnerService(IGenericRepository<CarOwner> _carOwnerRepository) : ICarOwnerService
    {
        public async Task<PagedResult<CarOwner>> GetCarOwnersAsync(PFSParameters pfs)
        {
            var query = _carOwnerRepository.GetQuery(pfs);

            var totalCount = query.Count();

            if (pfs.Paging.PageSize > 0)
            {
                query = query
                    .Skip((pfs.Paging.PageNumber - 1) * pfs.Paging.PageSize)
                    .Take(pfs.Paging.PageSize);
            }

            var items = query.ToList();

            return new PagedResult<CarOwner>
            {
                Items = items,
                TotalCount = totalCount,
                Paging = pfs.Paging
            };
        }

        public async Task<CarOwner> GetCarOwnerByIdAsync(int id)
        {
            var carOwner = await _carOwnerRepository.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException($"CarOwner with ID {id} not found.");
            return carOwner;
        }

        public async Task<CarOwner> AddCarOwnerAsync(CarOwner carOwner)
        {
            ArgumentNullException.ThrowIfNull(carOwner);

            var potentialMatches = _carOwnerRepository.GetQuery(new PFSParameters
            {
                Filter = new FilterParameters { PropertyName = "LastName", Filter = carOwner.LastName }
            }).ToList();

            var existing = potentialMatches.FirstOrDefault(c =>
                c.FirstName.Equals(carOwner.FirstName, System.StringComparison.OrdinalIgnoreCase) &&
                c.LastName.Equals(carOwner.LastName, System.StringComparison.OrdinalIgnoreCase) &&
                c.DateOfBirth == carOwner.DateOfBirth
            );

            if (existing != null)
                throw new System.Exception($"CarOwner {carOwner.FirstName} {carOwner.LastName} with the same date of birth already exists.");

            return await _carOwnerRepository.AddAsync(carOwner);
        }

        public async Task<CarOwner> UpdateCarOwnerAsync(int id, CarOwner carOwner)
        {
            ArgumentNullException.ThrowIfNull(carOwner);

            var existing = await _carOwnerRepository.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException($"CarOwner with ID {id} not found.");

            existing.FirstName = carOwner.FirstName;
            existing.LastName = carOwner.LastName;
            existing.DateOfBirth = carOwner.DateOfBirth;

            return await _carOwnerRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteCarOwnerAsync(int id)
        {
            return await _carOwnerRepository.DeleteAsync(id);
        }
    }
}