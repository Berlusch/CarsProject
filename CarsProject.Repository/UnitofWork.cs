using CarsProject.DAL;
using CarsProject.Repository.Common;

namespace CarsProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarsDbContext _context;

        public ICarMakeRepository CarMakeRepository { get; }
        public ICarModelRepository CarModelRepository { get; }
        public ICarOwnerRepository CarOwnerRepository { get; }
        public ICarRegistrationRepository CarRegistrationRepository { get; }
        public ICarEngineTypeRepository CarEngineTypeRepository { get; }

        public UnitOfWork(
            CarsDbContext context,
            ICarMakeRepository carMakeRepository,
            ICarModelRepository carModelRepository,
            ICarOwnerRepository carOwnerRepository,
            ICarRegistrationRepository carRegistrationRepository,
            ICarEngineTypeRepository carEngineTypeRepository)
        {
            _context = context;
            CarMakeRepository = carMakeRepository;
            CarModelRepository = carModelRepository;
            CarOwnerRepository = carOwnerRepository;
            CarRegistrationRepository = carRegistrationRepository;
            CarEngineTypeRepository = carEngineTypeRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

