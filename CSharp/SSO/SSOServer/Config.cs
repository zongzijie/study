using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace SSOServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("api","My Api App")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "mvc",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets ={
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = false,
                    RedirectUris = {"http://localhost:5001/signin-oidc",
                        "http://localhost:5002/signin-oidc" } ,
                    PostLogoutRedirectUris = {"http://localhost:5001/signout-callback-oidc" ,
                        "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                }
            };
        }
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "10000",
                    Username = "zara",
                    Password = "112233"
                }
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}