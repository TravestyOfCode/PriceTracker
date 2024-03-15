using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace PriceTracker.Data.User;

public class UserManager : IUserManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly ILogger<UserManager> _logger;

    public UserManager(IServiceProvider services, ILogger<UserManager> logger)
    {
        _signInManager = services.GetRequiredService<SignInManager<ApplicationUser>>();
        _logger = logger;
    }

    public bool IsSignedIn(ClaimsPrincipal user) => _signInManager.IsSignedIn(user);

    public async Task<SignInResult> LoginByEmailAsync(string email, string password, bool isPersistant, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogError("User: {email} not found when trying to login.", email);

                return SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(user, password, isPersistant, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return SignInResult.Failed;
        }
    }

    public Task Logout() => _signInManager.SignOutAsync();
}
