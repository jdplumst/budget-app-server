using System.Diagnostics.CodeAnalysis;

namespace BudgetApp.Models
{

    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }

        [SetsRequiredMembers]
        public Project(string Name)
        {
            this.Name = Name;
        }
    }
}