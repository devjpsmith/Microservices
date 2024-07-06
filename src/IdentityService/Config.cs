using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("auctionsApi", "Auction API")
            {
                Scopes = { "internal", "auctionApp" }
            },
            new ApiResource("financialsApi", "Cash Money")
            {
                Scopes = { "read.financials" }
            }
        };
    
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("auctionApp", "Auction app access"),
            new ApiScope("internal", "M2M Internal Services"),
            new ApiScope("read.financials", "Financial Api access")
        };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",
            
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = [ "internal", "read.financials" ]
            },

            // interactive client using code flow + pkce
            // new Client
            // {
            //     ClientId = "interactive",
            //     ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
            //
            //     AllowedGrantTypes = GrantTypes.Code,
            //
            //     RedirectUris = { "https://localhost:44300/signin-oidc" },
            //     FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            //     PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },
            //
            //     AllowOfflineAccess = true,
            //     AllowedScopes = { "openid", "profile", "scope2" }
            // },
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = [ "openid", "profile", "auctionApp" ],
                RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                ClientSecrets = { new Secret("not-a-secret".Sha256())},
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword }
            }
        };
}
