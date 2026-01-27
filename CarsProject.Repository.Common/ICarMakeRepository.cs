using CarsProject.Common;
using CarsProject.Model;
using CarsProject.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsProject.Repository.Common
{
    public interface ICarMakeRepository : IGenericRepository<CarMake>
    {
        Task<IEnumerable<CarMake>> GetAllCarMakesAsync(PSFParameters psf);
    }
}

