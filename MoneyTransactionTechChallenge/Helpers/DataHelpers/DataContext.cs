using System.Data;
using Dapper;
using Npgsql;

namespace MoneyTransactionTechChallenge.Helpers.DataHelpers;

public class DataContext
{
    public IDbConnection CreateConnection()
    {
        var connectionString  = "Host=localhost;Port=5431;Database=MoneyTransaction;Username=postgres;Password=password";
        return new NpgsqlConnection(connectionString);
    }

    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    private async Task _initDatabase()
    {
        var connectionString  = "Host=localhost;Port=5431;Database=MoneyTransaction;Username=postgres;Password=password";
        using var connection =  new NpgsqlConnection(connectionString);
        
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = 'MoneyTransaction'";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);

        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE MoneyTransaction";
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task _initTables()
    {
        using var connection = CreateConnection();
        await _initUsers();
        // await _initWallets();

        async Task _initUsers()
        {
            var sql = """
                      CREATE TABLE IF NOT EXISTS users (
                          id VARCHAR PRIMARY KEY,
                          first_name VARCHAR,
                          last_name VARCHAR,
                          email VARCHAR,
                          password VARCHAR,
                          cpf VARCHAR,
                          user_type VARCHAR
                      );
                      """;
            await connection.ExecuteAsync(sql);
        }
        
        async Task _initWallets()
        {
            var sql = """
                      CREATE TABLE IF NOT EXISTS wallet (
                          wallet_id VARCHAR PRIMARY KEY,
                          balance DECIMAL,
                          user_id VARCHAR,
                          CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users(id)
                      );
                      """;
            await connection.ExecuteAsync(sql);
        }
    }
}