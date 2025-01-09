using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace TeduMicroservices.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string>{"role"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("tedu-microservice_api.read", "Read Access to TeduMicroservice API"),
                new ApiScope("tedu-microservice_api.write", "Write Access to TeduMicroservice API"),
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new ApiResource("tedu-microservice_api")
                {
                    Scopes = { "tedu-microservice_api.read", "tedu-microservice_api.write" },
                    UserClaims = { "roles" }
                }
            };

    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new Client()
                {
                    ClientName = "TeduMicroservice Swagger",
                    ClientId = "tedu-microservice_swagger",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 60 * 60 * 2,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:5001/swagger/oauth2-redirect.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:5001/swagger/oauth2-redirect.html"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:5001"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "tedu-microservice_api.read",
                        "tedu-microservice_api.write",
                        "role"
                    }
                },
                new Client()
                {
                    ClientName = "TeduMicroservice Postman",
                    ClientId = "tedu-microservice_postman",
                    Enabled = true,
                    ClientUri = null,
                    RequireClientSecret = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SuperStrongSecret".Sha512())
                    },
                    AllowedGrantTypes = new List<string>
                    {
                        GrantType.ClientCredentials,
                    },
                    RequireConsent = false,
                    AccessTokenLifetime = 60 * 60 * 2,
                    AllowOfflineAccess = true,
                    RedirectUris = new List<string>
                    {
                        "https://www.getpostman.com/oauth2/callback"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "tedu-microservice_api.read",
                        "tedu-microservice_api.write",
                        "role"
                    }
                }
            };


}