using CarsProject.WebApi;

namespace CarsProject.Repository.Common
{
    public interface IGenericRepository<T> where T : class
    {
        
        Task<PagedResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null); 

        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}



