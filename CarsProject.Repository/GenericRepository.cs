using CarsProject.DAL;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CarsDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CarsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        
        public async Task<PagedResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _dbSet;

           
            if (filter != null)
            {
                query = filter(query);
            }

            
            if (orderBy != null)
            {
                query = orderBy(query);
            }

           
            var totalItems = await query.CountAsync();

            
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                TotalCount = totalItems,  
                Items = items              
            };
        }
        
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            return entity;
        }

        
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

       
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        Task<WebApi.PagedResult<T>> IGenericRepository<T>.GetPagedAsync
            (int pageNumber, int pageSize, Func<IQueryable<T>, IQueryable<T>>? filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy)
        {
            throw new NotImplementedException();
        }
    }

    
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }  
        public required IEnumerable<T> Items { get; set; } 
    }
}
