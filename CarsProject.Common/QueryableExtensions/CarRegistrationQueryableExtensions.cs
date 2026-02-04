using CarsProject.Model;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Common.QueryableExtensions
{
    public static class CarRegistrationQueryableExtensions
    {
        public static IQueryable<CarRegistration> IncludeFKs(this IQueryable<CarRegistration> query)
        {
            return query
                .Include(cm => cm.CarOwner)
                .Include(cm => cm.CarModel);
        }
    }
}
