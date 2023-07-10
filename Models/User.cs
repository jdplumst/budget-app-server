using System.Diagnostics.CodeAnalysis;

namespace BudgetApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.User;
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        [SetsRequiredMembers]
        public User(string username, string passwordHash)
        {
            this.Username = username;
            this.PasswordHash = passwordHash;
        }
    }

    [Flags]
    public enum Role
    {
        User = 0,
        Premium = 1,
        Admin = 2
    }
}