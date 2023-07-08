using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public class BudgetAppContext : DbContext
    {
        public BudgetAppContext(DbContextOptions<BudgetAppContext> options) : base(options)
        { }
        public required DbSet<User> Users { get; set; }
        public required DbSet<Project> Projects { get; set; }
        public required DbSet<Expense> Expenses { get; set; }
    }
}