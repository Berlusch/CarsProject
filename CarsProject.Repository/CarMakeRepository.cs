using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsProject.Repository
{
    public class CarMakeRepository : GenericRepository<CarMake>, ICarMakeRepository
    {
        private readonly CarsDbContext _context;

        // Konstruktori
        public CarMakeRepository(CarsDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementacija metode za preuzimanje svih CarMakes podataka
        public async Task<IEnumerable<CarMake>> GetAllCarMakesAsync()
        {
            return await _context.CarMakes.ToListAsync(); // Dohvati sve CarMakes iz baze
        }
    }
}


