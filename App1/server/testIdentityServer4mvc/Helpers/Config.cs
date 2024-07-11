using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

public static class Config
{
    public static IEnumerable<Client> Clients => new Client[]
    {
        new Client
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = { "api1" }
        },
        new Client
        {
            ClientId = "pkce_client",
            //ClientSecrets={new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true, 
            // RedirectUris = { "https://localhost:5212/signin-oidc" },

            //     // where to redirect to after logout
            //     PostLogoutRedirectUris = { "https://localhost:5212/signout-callback-oidc" },
            RedirectUris = { "http://localhost:4201/callback", "https://localhost:5212/signin-oidc" },
            PostLogoutRedirectUris = { "http://localhost:4201" },
            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "testIdentityServer4mvc"
            },
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
            AllowedCorsOrigins = { "http://localhost:4201" },
            RequireClientSecret = false
        }
    };

    public static IEnumerable<ApiScope> ApiScopes =>
    new ApiScope[]
    {
        new ApiScope("testIdentityServer4mvc", "testIdentityServer4mvc")
    };

    public static List<TestUser> Users => new List<TestUser>
    {
        new TestUser
        {
            Username = "alice",
            Claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com")
            },
            Password = "Abcd@1234",
            IsActive = true,
            SubjectId = Guid.NewGuid().ToString(),
        }
    };

    //public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    //{
    //    new ApiScope("api1", "My API")
    //};

    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            ApiResource
        };
    }

    public static ApiResource ApiResource => new ApiResource("testIdentityServer4mvc")
    {
        Scopes = new List<string>
        {
            "testIdentityServer4mvc"
        }
    };
}
