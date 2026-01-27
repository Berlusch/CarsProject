using CarsProject.WebApi;

namespace CarsProject.Repository.Common
{
    public interface ICarRegistrationRepository : IGenericRepository<CarRegistration>

    {
        Task<IEnumerable<CarRegistration>> GetAllCarRegistrationsAsync();
    }
}
