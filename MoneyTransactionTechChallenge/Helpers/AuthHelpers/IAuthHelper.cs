using MoneyTransactionTechChallenge.Models;

namespace MoneyTransactionTechChallenge.Helpers.AuthHelpers;

public interface IAuthHelper
{
    string GenerateJwtToken(User user);
    string GetCurrentUserId(string jwtToken);
}