using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransactionTechChallenge.Models.InputModels;
using MoneyTransactionTechChallenge.Services;

namespace MoneyTransactionTechChallenge.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public UserController(IUserService userService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("api/user/register")]
    public async Task<IActionResult> CreateUser([FromBody] UserInputModel userInputModel)
    {
        await _userService.CreateUser(userInputModel);
        
        return Ok(new { message = "User created successfully!" }); 
    }

    [HttpGet]
    [Route("api/user/getById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("api/user/getByEmail/{email}")]
    public async Task<IActionResult> GetByEmail([FromRoute] string email)
    {
        var result = await _userService.GetUserByEmailAsync(email);
        
        return Ok(result);
    }
    
    [HttpPost]
    [Route("api/user/Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _authenticationService.AuthenticateUser(email, password);
        
        return Ok(result);
    }
}