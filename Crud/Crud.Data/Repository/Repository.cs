using Crud.Data.Context;
using Crud.Data.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ContextDB context;
        protected DbSet<TEntity> dbset;

        public Repository(ContextDB context)
        {
            this.context = context;
            dbset = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            dbset.Add(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = dbset;
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await dbset.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            dbset.Remove(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            dbset.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
