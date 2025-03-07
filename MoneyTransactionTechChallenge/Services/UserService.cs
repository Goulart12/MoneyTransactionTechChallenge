using System.Text.RegularExpressions;
using MoneyTransactionTechChallenge.Models;
using MoneyTransactionTechChallenge.Models.InputModels;
using MoneyTransactionTechChallenge.Repositories;
using BCrypt.Net;
using static BCrypt.Net.BCrypt;

namespace MoneyTransactionTechChallenge.Services;

public partial class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    private const int WorkFactor = 12;
    private const string PasswordValidationRegEx = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-+_.]).{8,16}$";

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUser(UserInputModel inputModel)
    {
        var checkEmail = await _userRepository.GetByEmailAsync(inputModel.Email);
        var checkCpf = await _userRepository.GetByCpfAsync(inputModel.CPF);

        if (checkEmail != null || checkCpf != null)
        {
            throw new ApplicationException("User already exists.");
        }

        if (!MyRegex().IsMatch(inputModel.Password))
        {
            throw new ApplicationException("Invalid password.");
        }
        
        var hash = HashPassword(inputModel.Password, WorkFactor);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = inputModel.Email,
            First_Name = inputModel.FirstName,
            Last_Name = inputModel.LastName,
            Password = hash,
            User_Type = inputModel.Role,
            CPF = inputModel.CPF,
        };
        
        await _userRepository.CreateAsync(user);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        var user = _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new ApplicationException("User not found.");
        }

        return user;
    }

    public Task<User?> GetUserByIdAsync(string id)
    {
        var user = _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new ApplicationException("User not found.");
        }
        
        return user;
    }
    
    [GeneratedRegex(PasswordValidationRegEx)]
    private static partial Regex MyRegex();
}