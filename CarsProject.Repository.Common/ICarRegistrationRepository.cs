using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarRegistrationRepository : ICRUDRepository<CarRegistration>

    {
        Task<IEnumerable<CarRegistration>> GetAllCarRegistrationsAsync();
    }
}
