using System;
using System.Threading.Tasks;

namespace CarsProject.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        ICarMakeRepository CarMakeRepository { get; }
        ICarModelRepository CarModelRepository { get; }
        ICarOwnerRepository CarOwnerRepository { get; }
        ICarRegistrationRepository CarRegistrationRepository { get; }
        ICarEngineTypeRepository CarEngineTypeRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
