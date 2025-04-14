using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;

namespace CarsProject.Service
{
    public class CarOwnerService : ICarOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarOwnerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // PFS (pagination, filtering, sorting)
        public async Task<IEnumerable<CarOwnerDTORead>> GetCarOwnersPagedAsync(int pageNumber, int pageSize, string sortBy, string filter)
        {
            var carOwnersQuery = await _unitOfWork.CarOwnerRepository.GetAllCarOwnersAsync();

            // Filtering
            if (!string.IsNullOrEmpty(filter))
            {
                string lowerFilter = filter.ToLower();
                carOwnersQuery = carOwnersQuery.Where(c => c.LastName.ToLower().Contains(lowerFilter));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "last name")
                {
                    carOwnersQuery = carOwnersQuery.OrderBy(c => c.LastName);
                }
                else
                {
                    carOwnersQuery = carOwnersQuery.OrderBy(c => c.Id);
                }
            }

            // Pagination
            var carOwnersPaged = carOwnersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            
            var result = _mapper.Map<IEnumerable<CarOwnerDTORead>>(carOwnersPaged);

            return result;
        }

        public async Task<CarOwnerDTORead> GetCarOwnerByIdAsync(int id)
        {
            var carOwner = await _unitOfWork.CarOwnerRepository.GetByIdAsync(id);

            if (carOwner == null)
            {
                throw new Exception($"CarOwner with ID {id} not found.");
            }

            return _mapper.Map<CarOwnerDTORead>(carOwner);
        }

        public async Task<CarOwnerDTORead> AddCarOwnerAsync(CarOwnerDTOInsertUpdate carOwnerDto)
        {
            
            var carOwners = await _unitOfWork.CarOwnerRepository.GetAllCarOwnersAsync(); 

            var existingCarOwner = carOwners.FirstOrDefault(c => c.LastName.Equals(carOwnerDto.LastName, StringComparison.OrdinalIgnoreCase));

            if (existingCarOwner != null)
            {
                throw new Exception($"CarOwner with the name {carOwnerDto.LastName} already exists.");
            }

            
            var carOwner = _mapper.Map<CarOwner>(carOwnerDto);

            var addedCarOwner = await _unitOfWork.CarOwnerRepository.AddAsync(carOwner);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarOwnerDTORead>(addedCarOwner);
        }

        public async Task<CarOwnerDTORead> UpdateCarOwnerAsync(int id, CarOwnerDTOInsertUpdate carOwnerDto)
        {
            var existingCarOwner = await _unitOfWork.CarOwnerRepository.GetByIdAsync(id);

            if (existingCarOwner == null)
            {
                throw new KeyNotFoundException($"CarOwner with ID {id} not found.");
            }

            
            _mapper.Map(carOwnerDto, existingCarOwner);

            var updatedCarOwner = await _unitOfWork.CarOwnerRepository.UpdateAsync(existingCarOwner);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarOwnerDTORead>(updatedCarOwner);
        }

        public async Task<bool> DeleteCarOwnerAsync(int id)
        {
            var existingCarOwner = await _unitOfWork.CarOwnerRepository.GetByIdAsync(id);
            if (existingCarOwner == null)
            {
                return false; 
            }

            var deleted = await _unitOfWork.CarOwnerRepository.DeleteAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;        }

        
    }
}

