using MoneyTransactionTechChallenge.Models;

namespace MoneyTransactionTechChallenge.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByCpfAsync(string cpf);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
}