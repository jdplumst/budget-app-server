using System.Diagnostics.CodeAnalysis;

namespace BudgetApp.Models
{
  public class Expense
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required ExpenseType Type { get; set; }
    public required decimal Amount { get; set; }
    public int ProjectId { get; set; }

    [SetsRequiredMembers]
    public Expense(string name, ExpenseType type, decimal amount)
    {
      this.Name = name;
      this.Type = type;
      this.Amount = amount;
    }
  }

  [Flags]
  public enum ExpenseType
  {
    Housing = 0,
    Transportation = 1,
    Food = 2,
    Utilities = 3,
    Insurance = 4,
    Healthcare = 5,
    Debt = 6,
    Clothing = 7,
    Supplies = 8,
    Personal = 9,
    Education = 10,
    Gifts = 11,
    Entertainment = 12,
    Retirement = 13,
    Miscellaneous = 14
  }
}