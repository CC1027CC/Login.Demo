using Login.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Login.Demo.Domain
{
    public class MyDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public Task<bool> AnyAsync { get; internal set; }

        public MyDbContext(DbContextOptions options):base(options)
        {

        }
    }
}
