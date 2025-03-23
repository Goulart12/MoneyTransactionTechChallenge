using MoneyTransactionTechChallenge.Helpers.AuthHelpers;
using MoneyTransactionTechChallenge.Helpers.CacheHelpers;
using MoneyTransactionTechChallenge.Repositories;
using BCrypt.Net;
using StackExchange.Redis;
using static BCrypt.Net.BCrypt;

namespace MoneyTransactionTechChallenge.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthHelper _authHelper;
    private readonly IRedisCacheService _redisCacheService;

    public AuthenticationService(IUserRepository userRepository, IAuthHelper authHelper, IRedisCacheService redisCacheService)
    {
        _userRepository = userRepository;
        _authHelper = authHelper;
        _redisCacheService = redisCacheService;
    }

    public async Task<string> AuthenticateUser(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!Verify(password, user.Password))
        {
            throw new Exception("Invalid password");
        }

        var token = _authHelper.GenerateJwtToken(user);
        
        var cacheKey = $"Token_JWT";

        var tokenCache = _redisCacheService.GetCachedData<string>(cacheKey);

        if (tokenCache != null)
        {
            _redisCacheService.RemoveCachedData(cacheKey);
        }
        
        _redisCacheService.SetCachedData(cacheKey, token, TimeSpan.FromMinutes(30));

        return token;
    }
}