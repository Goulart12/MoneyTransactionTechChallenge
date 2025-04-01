using System.Text.Json;
using System.Text.Json.Nodes;
using MoneyTransactionTechChallenge.Helpers.AuthHelpers;
using MoneyTransactionTechChallenge.Helpers.CacheHelpers;
using MoneyTransactionTechChallenge.Models;
using MoneyTransactionTechChallenge.Models.FinancialTransactions;
using MoneyTransactionTechChallenge.Repositories.FinancialTransactions;
using RestSharp;

namespace MoneyTransactionTechChallenge.Services.FinancialTransactions;

public class FinancialTransactionService : IFinancialTransactionService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IAuthHelper _authHelper;
    private readonly IRedisCacheService _RedisCacheService;
    private readonly HttpClient _httpClient;

    private const decimal InitialBalance = 0;

    public FinancialTransactionService(IWalletRepository walletRepository, IAuthHelper authHelper, IRedisCacheService redisCacheService, HttpClient httpClient)
    {
        _walletRepository = walletRepository;
        _authHelper = authHelper;
        _RedisCacheService = redisCacheService;
        _httpClient = httpClient;
    }

    public async Task CreateWalletAsync()
    {
        var cacheKey = $"Token_JWT";
        var tokenCache = _RedisCacheService.GetCachedData<string>(cacheKey);

        var userId = _authHelper.GetCurrentUserId(tokenCache);

        var wallet = new Wallet
        {
            Wallet_Id = Guid.NewGuid().ToString(),
            Balance = InitialBalance,
            User_Id = userId
        };
        
        await _walletRepository.CreateWalletAsync(wallet);
    }

    public async Task<Wallet> GetWalletByIdAsync(string walletId)
    {
        var wallet = await _walletRepository.GetWalletByIdAsync(walletId);

        if (wallet == null)
        {
            throw new ApplicationException("Wallet not found.");
        }
        
        return wallet;
    }

    public async Task<Wallet> GetWalletByUserAsync()
    {
        var cacheKey = $"Token_JWT";
        var tokenCache = _RedisCacheService.GetCachedData<string>(cacheKey);

        var userId = _authHelper.GetCurrentUserId(tokenCache);
 
        var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
        
        if (wallet == null)
        {
            throw new ApplicationException("Wallet not found.");
        }
        
        return wallet;
    }

    public async Task UpdateBalanceAsync(string userId, decimal balance)
    {
        var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);

        if (wallet == null)
        {
            throw new ApplicationException("Wallet not found.");
        }
        
        var oldBalance = wallet.Balance;
        var newBalance = wallet.Balance + balance;
        wallet.Balance = newBalance;
        
        await _walletRepository.UpdateWalletAsync(wallet);
    }

    public async Task UsersTransactionAsync(string payeeId, decimal amount)
    {
        var cacheKey = $"Token_JWT";
        var tokenCache = _RedisCacheService.GetCachedData<string>(cacheKey);
        var payerId = _authHelper.GetCurrentUserId(tokenCache);
        var payerRole = _authHelper.GetCurrentUserRole(tokenCache);

        if (payerRole != "Usuario")
        {
            throw new ApplicationException("You are not authorized to perform this action.");
        }
        
        var payerWallet = await _walletRepository.GetWalletByUserIdAsync(payerId);
        var payeeWallet = await _walletRepository.GetWalletByUserIdAsync(payeeId);

        if (payerWallet == null || payeeWallet == null)
        {
            throw new ApplicationException("Wallet not found.");
        }

        if (payerWallet.Balance < amount)
        {
            throw new ApplicationException("Insufficient balance.");
        }
        
        var newPayerBalance = payerWallet.Balance - amount;
        var newPayeeBalance = payeeWallet.Balance + amount;
        
        payerWallet.Balance = newPayerBalance;
        payeeWallet.Balance = newPayeeBalance;

        var url = "https://util.devi.tools/api/v2/authorize";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException("Unable to connect to the authorization process.");
        }
        
        await _walletRepository.UpdateWalletAsync(payerWallet);
        await _walletRepository.UpdateWalletAsync(payeeWallet);
        
        var clientNotification = new RestClient($"https://util.devi.tools/api/v1/notify)");
        var requestNotification = new RestRequest("");
        var responseNotification = await clientNotification.PostAsync(requestNotification);
        
        string jsonContent = "{\"key\": \"value\"}";
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var messageResponse = await _httpClient.PostAsync("https://util.devi.tools/api/v1/notify)", content);

        if (!messageResponse.IsSuccessStatusCode)
        {
            throw new ApplicationException("Unable to connect to the notification process.");
        }
    }
}