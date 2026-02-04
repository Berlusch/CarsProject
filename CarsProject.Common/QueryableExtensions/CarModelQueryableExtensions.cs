using CarsProject.Model;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Common.QueryableExtensions
{
    public static class CarModelQueryableExtensions
    {
        public static IQueryable<CarModel> IncludeFKs(this IQueryable<CarModel> query)
        {
            return query
                .Include(cm => cm.CarMake)
                .Include(cm => cm.CarEngineType);
        }
    }
}