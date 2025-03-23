using MoneyTransactionTechChallenge.Models;

namespace MoneyTransactionTechChallenge.Services.FinancialTransactions;

public interface IFinancialTransactionService
{
    Task CreateWalletAsync();
    Task<Wallet> GetWalletByIdAsync(string walletId);
    Task<Wallet> GetWalletByUserAsync();
    Task UpdateBalanceAsync(string userId, decimal balance);
    Task UsersTransactionAsync(string payeeId, decimal amount);
}