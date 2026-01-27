using CarsProject.Common;
using CarsProject.DAL;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CarsProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CarsDbContext _context;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CarsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetQuery(PSFParameters parameters)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrEmpty(parameters.Filter.Filter) &&
                !string.IsNullOrEmpty(parameters.Filter.PropertyName))
            {
                var propInfo = typeof(T).GetProperty(parameters.Filter.PropertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null && propInfo.PropertyType == typeof(string))
                {
                    query = query.Where(e =>
                        EF.Property<string>(e, parameters.Filter.PropertyName)
                          .Contains(parameters.Filter.Filter));
                }
            }

            if (!string.IsNullOrEmpty(parameters.Sorting.OrderBy))
            {
                var propInfo = typeof(T).GetProperty(parameters.Sorting.OrderBy,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null)
                {
                    query = parameters.Sorting.Descending
                        ? query.OrderByDescending(x => propInfo.GetValue(x, null))
                        : query.OrderBy(x => propInfo.GetValue(x, null));
                }
            }

            return query;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
