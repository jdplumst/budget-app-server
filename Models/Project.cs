using System.Diagnostics.CodeAnalysis;

namespace BudgetApp.Models
{

    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Budget { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int UserId { get; set; }
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        [SetsRequiredMembers]
        public Project(string Name, decimal Budget)
        {
            this.Name = Name;
            this.Budget = Budget;
        }
    }
}