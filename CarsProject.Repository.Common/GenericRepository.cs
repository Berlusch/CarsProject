using CarsProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarsProject.Repository.Common
{
    public class GenericRepository<T> : ICRUDRepository<T> where T : class
    {
        private readonly CarsDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CarsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /*public async Task<List<T>> ListAsync()
        {
            // dohvaća sve
            return await dbContext.Set<T>().ToListAsync();
        }*/


        
        public async Task<T> GetByIdAsync(int id)
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
    }
}
