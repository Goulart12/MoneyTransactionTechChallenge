using MoneyTransactionTechChallenge.Models;
using MoneyTransactionTechChallenge.Models.Enums;
using MoneyTransactionTechChallenge.Models.InputModels;
using MoneyTransactionTechChallenge.Repositories;
using MoneyTransactionTechChallenge.Services;
using Moq;
using Shouldly;
using Xunit;

namespace MoneyTransactionTechChallenge.Tests;

public class UserTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService target;

    public UserTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        
        target = new UserService(_userRepositoryMock.Object);
    }


    [Fact]
    public void CreateUser_UserIsCreated()
    {
        var user = MockUserInputModel();

        var userService = target.CreateUser(user);

        userService.ShouldNotBeNull();
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void CreateUser_PasswordIsInvalid()
    {
        var user = new UserInputModel()
        {
            CPF = "00000000001",
            Email = "test@test.com",
            Password = "password123",
            FirstName = "Test",
            LastName = "Test",
            Role = Role.Usuario,
        };
        
        var userService = target.CreateUser(user);
        
        userService.ShouldThrow<ApplicationException>("Invalid password.");
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public void CreateUser_EmailAlreadyExists()
    {
        var userOne = CreateValidUser();
        
        var userTwo = new UserInputModel()
        {
            CPF = "00000000002",
            Email = "test@test.com",
            Password = "Password#134",
            FirstName = "TestTwo",
            LastName = "TestTwo",
            Role = Role.Usuario,
        };
        
        var userServiceTwo = target.CreateUser(userTwo);
        
        userServiceTwo.ShouldThrow<ApplicationException>("User already exists.");
    }
    
    [Fact]
    public void GetUser_UserExists()
    {
        var user = MockUserInputModel();

        var userService = target.CreateUser(user);

        userService.ShouldNotBeNull();
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    private User SetUpGetUser()
    {
        var user = CreateValidUser();
        
        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(user.Email)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.GetByCpfAsync(user.CPF)).ReturnsAsync(user);

        return user ?? throw new NullReferenceException();
    }

    private static User CreateValidUser()
    {
        var user = new User()
        {
            CPF = "00000000001",
            Email = "test@test.com",
            Password = "Password#123",
            First_Name = "Test",
            Last_Name = "Test",
            User_Type = Role.Usuario,
            Id = Guid.NewGuid().ToString(),
        };
        
        return user;
    }

    private UserInputModel MockUserInputModel()
    {
        var user = new UserInputModel()
        {
            CPF = "00000000001",
            Email = "test@test.com",
            Password = "Password#123",
            FirstName = "Test",
            LastName = "Test",
            Role = Role.Usuario,
        };
        
        return user;
    }
}