using System.Diagnostics.CodeAnalysis;

namespace BudgetApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        [SetsRequiredMembers]
        public User(string username, string passwordHash)
        {
            this.Username = username;
            this.PasswordHash = passwordHash;
        }
    }
}