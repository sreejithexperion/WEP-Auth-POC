using Duende.IdentityServer;
using Duende.IdentityServer.Models;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    }

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

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope("testIdentityServer4mvc", "testIdentityServer4mvc")
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "client_id",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1" }
            },
            new Client
            {
                ClientId = "token_exchange_client",
                AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "testIdentityServer4mvc" 
                },
                RequireClientSecret = false
            }
        };
    }
}