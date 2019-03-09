using Crud.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Context
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options)
            : base(options)

        { }

        public DbSet<User> User { get; set; }
    }
}
