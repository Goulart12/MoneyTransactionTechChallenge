using MoneyTransactionTechChallenge.Models;

namespace MoneyTransactionTechChallenge.Repositories.FinancialTransactions;

public interface IWalletRepository
{
    Task CreateWalletAsync(Wallet wallet);
    Task<Wallet?> GetWalletByIdAsync(string wallet_id);
    Task<Wallet?> GetWalletByUserIdAsync(string user_id);
    Task UpdateWalletAsync(Wallet wallet);
}