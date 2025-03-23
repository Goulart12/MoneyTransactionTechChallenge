namespace MoneyTransactionTechChallenge.Services;

public interface IAuthenticationService
{
    Task<string> AuthenticateUser(string email, string password);
}