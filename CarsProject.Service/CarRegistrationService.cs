using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;
using CarsProject.Service;

public class CarRegistrationService : ICarRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CarRegistrationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // PFS (pagination, filtering, sorting)
    public async Task<IEnumerable<CarRegistrationDTORead>> GetCarRegistrationsPagedAsync(int pageNumber, int pageSize, string sortBy, string filter)
    {
        var carRegistrationsQuery = await _unitOfWork.CarRegistrationRepository.GetAllCarRegistrationsAsync();

        // Filtering
        if (!string.IsNullOrEmpty(filter))
        {
            string lowerFilter = filter.ToLower();
            carRegistrationsQuery = carRegistrationsQuery.Where(c => c.RegistrationNumber.ToLower().Contains(lowerFilter));
        }

        // Sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.ToLower() == "registration number")
            {
                carRegistrationsQuery = carRegistrationsQuery.OrderBy(c => c.RegistrationNumber);
            }
            else
            {
                carRegistrationsQuery = carRegistrationsQuery.OrderBy(c => c.Id);
            }
        }

        // Pagination
        var carRegistrationsPaged = carRegistrationsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();  // Pretvori u List<CarRegistration>

        // Mapiraj List<CarRegistration> u List<CarRegistrationDTORead>
        var result = _mapper.Map<List<CarRegistrationDTORead>>(carRegistrationsPaged);

        return result;
    }


    public async Task<CarRegistrationDTORead> GetCarRegistrationByIdAsync(int id)
    {
        var carRegistration = await _unitOfWork.CarRegistrationRepository.GetByIdAsync(id);

        if (carRegistration == null)
        {
            throw new Exception($"CarRegistration with ID {id} not found.");
        }

        return _mapper.Map<CarRegistrationDTORead>(carRegistration);
    }

    public async Task<CarRegistrationDTORead> AddCarRegistrationAsync(CarRegistrationDTOInsertUpdate carRegistrationDto)
    {
        
        var carOwner = await _unitOfWork.CarOwnerRepository.GetByIdAsync(carRegistrationDto.CarOwnerId);
        if (carOwner == null)
        {
            throw new Exception($"CarOwner with ID {carRegistrationDto.CarOwnerId} not found.");
        }
                
        var carModel = await _unitOfWork.CarModelRepository.GetByIdAsync(carRegistrationDto.CarModelId);
        if (carModel == null)
        {
            throw new Exception($"CarModel with ID {carRegistrationDto.CarModelId} not found.");
        }

        // Check if the same CarRegistration already exists
        var carRegistrations = await _unitOfWork.CarRegistrationRepository.GetAllCarRegistrationsAsync();
        var existingCarRegistration = carRegistrations.FirstOrDefault(c => c.RegistrationNumber.Equals(carRegistrationDto.RegistrationNumber, StringComparison.OrdinalIgnoreCase));


        if (existingCarRegistration != null)
        {
            throw new Exception($"CarRegistration with the name {carRegistrationDto.RegistrationNumber} already exists.");
        }


        var carRegistration = _mapper.Map<CarRegistration>(carRegistrationDto);
        carRegistration.CarOwner = carOwner;
        carRegistration.CarModel = carModel;


        var addedCarRegistration = await _unitOfWork.CarRegistrationRepository.AddAsync(carRegistration);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CarRegistrationDTORead>(addedCarRegistration);
    }

    public async Task<CarRegistrationDTORead> UpdateCarRegistrationAsync(int id, CarRegistrationDTOInsertUpdate carRegistrationDto)
    {
        var existingCarRegistration = await _unitOfWork.CarRegistrationRepository.GetByIdAsync(id);

        if (existingCarRegistration == null)
        {
            throw new KeyNotFoundException($"CarRegistration with ID {id} not found.");
        }
                
        var carOwner = await _unitOfWork.CarOwnerRepository.GetByIdAsync(carRegistrationDto.CarOwnerId);
        if (carOwner == null)
        {
            throw new Exception($"CarOwner with ID {carRegistrationDto.CarOwnerId} not found.");
        }

        
        var carModel = await _unitOfWork.CarModelRepository.GetByIdAsync(carRegistrationDto.CarModelId);
        if (carModel == null)
        {
            throw new Exception($"CarModel with ID {carRegistrationDto.CarModelId} not found.");
        }

        
        _mapper.Map(carRegistrationDto, existingCarRegistration);
        existingCarRegistration.CarOwner = carOwner;  // Ažuriranje CarOwner
        existingCarRegistration.CarModel = carModel;  // Ažuriranje CarModel

        
        var updatedCarRegistration = await _unitOfWork.CarRegistrationRepository.UpdateAsync(existingCarRegistration);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CarRegistrationDTORead>(updatedCarRegistration);
    }

    public async Task<bool> DeleteCarRegistrationAsync(int id)
    {
        var existingCarRegistration = await _unitOfWork.CarRegistrationRepository.GetByIdAsync(id);
        if (existingCarRegistration == null)
        {
            return false;
        }

        var deleted = await _unitOfWork.CarRegistrationRepository.DeleteAsync(id);
        if (!deleted)
            return false;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

