using Hair.Core.Models;
using Hair.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hair.Core.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(x => !x.IsDeleted);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public IQueryable<T> GetAllWithThenInclude()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(x => !x.IsDeleted);
            query = query.Where(predicate);

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            var yeni = await query.FirstOrDefaultAsync();
            //var eski = await query.SingleOrDefaultAsync();

            return yeni;
        }
        
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => { _context.Set<T>().Update(entity); });
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => { _context.Set<T>().Remove(entity); });
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(x => !x.IsDeleted).AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(x => !x.IsDeleted).CountAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().Where(x => !x.IsDeleted).CountAsync();
        }
    }
}
