using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.In_Memory
{
    public class IdentityConfiguration
    {
        public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
        {
            SubjectId = "1144",
            Username = "Adeola",
            Password = "Adeola",
            Claims =
            {
                new Claim(JwtClaimTypes.Name, "Adeola Wura"),
                new Claim(JwtClaimTypes.GivenName, "Adeola"),
                new Claim(JwtClaimTypes.FamilyName, "Wura"),
                new Claim(JwtClaimTypes.WebSite, "https://github.com/Adexandria")
            }
        }
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
        new ApiScope("myApi.read","read"),
        new ApiScope("myApi.write","write"),
        new ApiScope("api1","apiname")
        };
        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
        new ApiResource("myApi")
        {
            Scopes = new List<string>{ "myApi.read","myApi.write,api1" },
            ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
        }
        };
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
             new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "api1","myApi.read","myApi.write" }
            }
        };
    }
}
