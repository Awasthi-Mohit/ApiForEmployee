using CrudApi.Model;
using Microsoft.EntityFrameworkCore;

namespace CrudApi.Data
{
    public class AppDbContext:DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
                
        }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
