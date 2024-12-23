using Dapper;
using MoneyTransactionTechChallenge.Helpers.DataHelpers;
using MoneyTransactionTechChallenge.Models;

namespace MoneyTransactionTechChallenge.Repositories;

public class UserRepository : IUserRepository
{
    private DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task CreateAsync(User user)
    {
        using var connection = _context.CreateConnection();
        var sql = """INSERT INTO users (id, first_name, last_name, email, password, cpf, user_type) VALUES (@Id, @FirstName, @LastName, @Email, @Password, @CPF, @Role) """;
        
        await connection.ExecuteAsync(sql, user);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = """SELECT * FROM users""";

        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        using var connection = _context.CreateConnection();
        var sql = """SELECT * FROM users WHERE id = @id""";
        
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();
        var sql = """SELECT * FROM users WHERE email = @email""";
        
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
    }
    
    public async Task<User?> GetByCpfAsync(string cpf)
    {
        using var connection = _context.CreateConnection();
        var sql = """SELECT * FROM users WHERE cpf = @cpf""";
        
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { cpf });
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = _context.CreateConnection();
        var sql = """UPDATE users SET first_name = @FirstName, last_name = @LastName, email = @Email, password = @Password, cpf = @CPF, user_type = @Role WHERE id = @Id """;
        
        await connection.ExecuteAsync(sql, user);
    }

    public async Task DeleteAsync(string id)
    {
        using var connection = _context.CreateConnection();
        var sql = """DELETE FROM users WHERE id = @Id""";
        
        await connection.ExecuteAsync(sql, new { id });
    }
}