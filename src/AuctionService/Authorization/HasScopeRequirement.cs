using Microsoft.AspNetCore.Authorization;

namespace AuctionService.Authorization;

public class HasScopeRequirement : IAuthorizationRequirement
{
    public string Issuer { get; set; }
    public string Scope { get; set; }

    public HasScopeRequirement(string scope, string issuer)
    {
        Scope = scope ?? throw new ArgumentException(nameof(scope));
        Issuer = issuer ?? throw new ArgumentException(nameof(issuer));
    }
}