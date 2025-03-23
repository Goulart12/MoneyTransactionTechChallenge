using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransactionTechChallenge.Services.FinancialTransactions;

namespace MoneyTransactionTechChallenge.Controllers;

public class FinancialTransactionController : ControllerBase
{
    private readonly IFinancialTransactionService _financialTransactionService;

    public FinancialTransactionController(IFinancialTransactionService financialTransactionService)
    {
        _financialTransactionService = financialTransactionService;
    }

    [Authorize]
    [HttpPost]
    [Route("api/financials/createWallet")]
    public async Task<IActionResult> CreateWallet()
    {
        await _financialTransactionService.CreateWalletAsync();
        
        return Ok(new { message = "Wallet created successfully!" });
    }

    [Authorize]
    [HttpGet]
    [Route("api/financials/getWalletById")]
    public async Task<IActionResult> GetWalletById(string id)
    {
        var result = await _financialTransactionService.GetWalletByIdAsync(id);
        
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet]
    [Route("api/financials/getWalletByUserId")]
    public async Task<IActionResult> GetWalletByUserId()
    {
        var result = await _financialTransactionService.GetWalletByUserAsync();
        
        return Ok(result);
    }

    [Authorize]
    [HttpPatch]
    [Route("api/financials/addBalance/{id}")]
    public async Task<IActionResult> AddBalance([FromRoute] string id, [FromBody] decimal amount)
    {
        await _financialTransactionService.UpdateBalanceAsync(id, amount);
        
        return Ok(new { message = "Balance updated successfully!" });
    }

    [Authorize]
    [HttpPatch]
    [Route("api/financials/usersTransaction/")]
    public async Task<IActionResult> UsersTransaction([FromBody] string id, decimal amount)
    {
        await _financialTransactionService.UsersTransactionAsync(id, amount);
        
        return Ok(new { message = "Transaction updated successfully!" });
    }
}