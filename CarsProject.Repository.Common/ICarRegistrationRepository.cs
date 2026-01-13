using CarsProject.WebApi;

namespace CarsProject.Repository.Common
{
    public interface ICarRegistrationRepository : ICRUDRepository<CarRegistration>

    {
        Task<IEnumerable<CarRegistration>> GetAllCarRegistrationsAsync();
    }
}
