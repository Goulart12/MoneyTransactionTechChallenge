using Dapper;
using MoneyTransactionTechChallenge.Helpers.DataHelpers;
using MoneyTransactionTechChallenge.Models;
using Npgsql;

namespace MoneyTransactionTechChallenge.Repositories;

public class UserRepository : IUserRepository
{
    private NpgsqlConnection _connection;
    public UserRepository()
    {
        _connection = new NpgsqlConnection("Host=localhost;Port=5431;Database=MoneyTransaction;Username=postgres;Password=password");
        _connection.Open();
    }
    
    public async Task CreateAsync(User user)
    {
        var sql = "INSERT INTO users (Id, FirstName, LastName, Email, Password, CPF, Role) VALUES (@id, @first_name, @last_name, @email, @password, @cpf, @user_type)";

        var queryArguments = new
        {
            id = user.Id,
            first_name = user.FirstName,
            last_name = user.LastName,
            email = user.Email,
            password = user.Password,
            cpf = user.CPF,
            user_type = user.Role
        };
        
        await _connection.ExecuteAsync(sql, queryArguments);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var sql = "SELECT * FROM users";

        return await _connection.QueryAsync<User>(sql);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var sql = "SELECT * FROM users WHERE id = @id";
        
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var sql = "SELECT * FROM users WHERE email = @email";
        
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
    }
    
    public async Task<User?> GetByCpfAsync(string cpf)
    {
        var sql = "SELECT * FROM users WHERE cpf = @cpf";
        
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { cpf });
    }

    public async Task UpdateAsync(User user)
    {
        var sql = "UPDATE users SET first_name = @FirstName, last_name = @LastName, email = @Email, password = @Password, cpf = @CPF, user_type = @Role WHERE id = @Id ";
        
        await _connection.ExecuteAsync(sql, user);
    }

    public async Task DeleteAsync(string id)
    {
        var sql = "DELETE FROM users WHERE id = @Id";
        
        await _connection.ExecuteAsync(sql, new { id });
    }
}