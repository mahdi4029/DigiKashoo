using Microsoft.EntityFrameworkCore;
using Dgk.Application.Interface;
using Dgk.Domain.User;

namespace Dgk.Persistence.Context
{
    public class DataBaseContext : DbContext,IDataBaseContext
    {
        public DbSet<User> Users { get; set; }

        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

    }
}
