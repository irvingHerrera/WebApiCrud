using Crud.Data.Context;
using Crud.Data.Entities;
using Crud.Data.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Data.Repository
{
    public class UnityOfWork : IUnityOfWork, IDisposable
    {
        private readonly ContextDB context;
        public IRepository<User> User => new Repository<User>(context);

        public UnityOfWork(ContextDB context)
        {
            this.context = context;
        }

        public void RejectChanges()
        {
            foreach (var entry in context.ChangeTracker.Entries()
              .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
