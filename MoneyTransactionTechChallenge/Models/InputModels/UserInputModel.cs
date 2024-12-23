using MoneyTransactionTechChallenge.Models.Enums;

namespace MoneyTransactionTechChallenge.Models.InputModels;

public class UserInputModel
{
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public Role Role { get; set; }
}