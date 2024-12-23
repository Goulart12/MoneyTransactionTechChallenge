using Microsoft.AspNetCore.Mvc;
using MoneyTransactionTechChallenge.Models.InputModels;
using MoneyTransactionTechChallenge.Services;

namespace MoneyTransactionTechChallenge.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("api/user/register")]
    public async Task<IActionResult> CreateUser(UserInputModel userInputModel)
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
    [Route("api/user/getByEmail/")]
    public async Task<IActionResult> GetByEmail([FromBody] string email)
    {
        var result = await _userService.GetUserByEmailAsync(email);
        
        return Ok(result);
    }
}