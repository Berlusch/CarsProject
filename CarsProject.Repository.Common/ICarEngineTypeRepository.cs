using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarEngineTypeRepository
    {
        Task<IEnumerable<CarEngineType>> GetAllAsync();        
        Task<CarEngineType> GetByIdAsync(Guid id);
    }
}
