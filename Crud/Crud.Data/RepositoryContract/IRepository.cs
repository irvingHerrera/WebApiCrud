using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crud.Data.RepositoryContract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}
