using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Datalayer
{
    public class IdentityUsersContext : IdentityDbContext
    {
        public IdentityUsersContext(DbContextOptions<IdentityUsersContext> options) : base(options){ }

        public static class InMemoryConfig
        {
            public static IEnumerable<IdentityResource> GetIdentityResources() =>
              new List<IdentityResource>
              {
                  new IdentityResources.OpenId(),
                  new IdentityResources.Profile(),
                  new IdentityResource("roles", "User role(s)", new[] { "role" })
              };

            public static List<TestUser> GetUsers() =>
                new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                        Username = "Mick",
                        Password = "MickPassword",                        
                        Claims = new List<Claim>
                        {
                            new Claim("given_name", "Mick"),
                            new Claim("family_name", "Mining"),
                            new Claim("role", "manager")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                        Username = "Jane",
                        Password = "JanePassword",
                        Claims = new List<Claim>
                        {
                            new Claim("given_name", "Jane"),
                            new Claim("family_name", "Downing"),
                            new Claim("role", "developer")
                        }
                    }
                };

            public static IEnumerable<Client> GetClients() =>
                new List<Client>
                {
                   new Client
                   {
                        ClientId = "company-employee",
                        ClientSecrets = new [] { new Secret("codemazesecret".Sha512()) },
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                        AllowedScopes = 
                        {
                           IdentityServerConstants.StandardScopes.OpenId,
                           "api"
                        }
                    },
                    new Client
                    {
                        ClientName = "MVC Client",
                        ClientId = "mvc-client",
                        AllowedGrantTypes = GrantTypes.Hybrid,
                        RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
                        RequirePkce = false,
                        AllowedScopes = 
                        { 
                            IdentityServerConstants.StandardScopes.OpenId, 
                            IdentityServerConstants.StandardScopes.Profile,
                            "roles"                           
                        },
                        ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                        PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" }
                    },
                    new Client
                    {
                        ClientId = "swaggerApi",
                        ClientName = "Swagger UI",
                        ClientSecrets = {new Secret("swggr".Sha256())}, 

                        AllowedGrantTypes = GrantTypes.Code,
                        //false?..
                        RequirePkce = false,
                        RequireClientSecret = false,
                        //5010 - api, 5005 - IS4
                        RedirectUris = {"https://localhost:5010/swagger/oauth2-redirect.html"},
                        AllowedCorsOrigins = {"https://localhost:5010"},
                        AllowedScopes = 
                        {
                            "api", 
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "roles"  
                        }
                    }
                };

            public static IEnumerable<ApiScope> GetApiScopes() =>
                new List<ApiScope> 
                { 
                    new ApiScope("api", "TicketManager API")
                };

            public static IEnumerable<ApiResource> GetApiResources() =>
                new List<ApiResource>
                {
                    new ApiResource("api", "Ticket Manager API")
                    {
                        Scopes = { "api" }
                    }
                };
        }
    }
}
