using Crud.Data.Entities;
using System.Threading.Tasks;

namespace Crud.Data.RepositoryContract
{
    public interface IUnityOfWork
    {
        IRepository<User> User { get; }
        Task<int> SaveChangesAsync();
        void RejectChanges();
        void Dispose();
    }
}
