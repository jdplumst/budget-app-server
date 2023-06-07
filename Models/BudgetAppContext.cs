using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public class BudgetAppContext : DbContext
    {
        public BudgetAppContext(DbContextOptions<BudgetAppContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}