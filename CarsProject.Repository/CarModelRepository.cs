using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        private readonly CarsDbContext _context;

        // Konstruktori
        public CarModelRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementacija metode za preuzimanje svih CarModels podataka
        public async Task<IEnumerable<CarModel>> GetAllCarModelsAsync()
        {
            return await _context.CarModels.ToListAsync(); // Dohvati sve CarModels iz baze
        }
    }
}



