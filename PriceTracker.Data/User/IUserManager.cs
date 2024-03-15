using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace PriceTracker.Data.User;

public interface IUserManager
{
    public bool IsSignedIn(ClaimsPrincipal user);

    public Task<SignInResult> LoginByEmailAsync(string email, string password, bool isPersistant, CancellationToken cancellationToken = default);

    public Task Logout();
}
