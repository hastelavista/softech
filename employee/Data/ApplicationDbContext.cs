using employee.Models;
using Microsoft.EntityFrameworkCore;

namespace employee.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Experience> Experiences { get; set; }
    }
}
