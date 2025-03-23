using Dapper;
using MoneyTransactionTechChallenge.Models;
using Npgsql;

namespace MoneyTransactionTechChallenge.Repositories.FinancialTransactions;

public class WalletRepository : IWalletRepository
{
    private NpgsqlConnection _connection;

    public WalletRepository()
    {
        _connection = new NpgsqlConnection("Host=localhost;Port=5431;Database=MoneyTransaction;Username=postgres;Password=password");
        _connection.Open();
    }

    public async Task CreateWalletAsync(Wallet wallet)
    {
        var table = "CREATE TABLE IF NOT EXISTS wallets (wallet_id VARCHAR PRIMARY KEY, balance DECIMAL, user_id VARCHAR, CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users(id))";
        await _connection.ExecuteAsync(table);
        
        var sql = "INSERT INTO wallets (wallet_id, balance, user_id) VALUES (@wallet_id, @balance, @user_id)";

        var queryArguments = new
        {
            wallet_id = wallet.Wallet_Id,
            balance = wallet.Balance,
            user_id = wallet.User_Id
        };
        
        await _connection.ExecuteAsync(sql, queryArguments);
    }

    public async Task<Wallet?> GetWalletByIdAsync(string wallet_id)
    {
        var sql = "SELECT * FROM wallets WHERE wallet_id = @wallet_id";
        
        return await _connection.QuerySingleOrDefaultAsync<Wallet>(sql, new { wallet_id });
    }

    public async Task<Wallet?> GetWalletByUserIdAsync(string user_id)
    {
        var sql = "SELECT * FROM wallets WHERE user_id = @user_id";
        
        return await _connection.QuerySingleOrDefaultAsync<Wallet>(sql, new { user_id });
    }

    public async Task UpdateWalletAsync(Wallet wallet)
    {
        var sql = "UPDATE wallets SET balance = @balance WHERE wallet_id = @wallet_id";
        
        await _connection.ExecuteAsync(sql, wallet);
    }
}