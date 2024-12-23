using MoneyTransactionTechChallenge.Models;
using MoneyTransactionTechChallenge.Models.InputModels;

namespace MoneyTransactionTechChallenge.Services;

public interface IUserService
{
    Task CreateUser(UserInputModel inputModel);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(string id);
}