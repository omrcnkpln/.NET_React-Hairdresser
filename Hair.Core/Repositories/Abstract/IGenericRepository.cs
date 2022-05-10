using Hair.Core.Models;
using System.Linq.Expressions;

namespace Hair.Core.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
            params Expression<Func<T, object>>[] includeProperties);

        // var kullanici = repository.GetAsync(k=>k.Id==15);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> AddAsync(T entity);
        
        Task<T> UpdateAsync(T entity);
        
        Task DeleteAsync(T entity);
        
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        
        Task<int> CountAsync();
        
        //ThenInclude ile sorgulama
        IQueryable<T> GetAllWithThenInclude();
    }
}
